using Bot.Extensions;
using Discord;
using Levels.Data;
using System.Text.RegularExpressions;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.Fonts;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using Image = SixLabors.ImageSharp.Image;
using Color = SixLabors.ImageSharp.Color;
using IOPath = System.IO.Path;
using SixLabors.ImageSharp.Drawing.Processing;
using Bot.Data;

namespace Levels.Models;

public static class Rankcard
{
	public static readonly Size RankCardSize = new(widthmain + pfpside, height);

	private const int widthmain = 1000;
	private const int height = 450;
	private const int pfpside = 350;
	private const int levelWidth = 950;
	private const int levelHeight = 125;
	private const int defMargin = 25;
	private static readonly Rectangle mainRect = new(0, 0, widthmain + pfpside, height);
	private static readonly Rectangle titleRect = new(defMargin, defMargin, widthmain - 2 * defMargin + pfpside, labelHeight);
	private static readonly LevelRect mainLevel = new(height - 2 * levelHeight - 2 * defMargin);
	private static readonly LevelRect secondaryLevel = new(height - levelHeight - defMargin);
	private static readonly LevelRect mainHybridLevel = new(height - levelHeight - defMargin, LevelRect.LevelBarType.HybridMain);
	private static readonly LevelRect secondaryHybridLevel = new(height - levelHeight - defMargin, LevelRect.LevelBarType.HybridSecondary);
	private static readonly Rectangle rectName = new(defMargin, defMargin, widthmain - 2 * defMargin + pfpside, labelHeight);
	private static readonly Rectangle rectPfp = new(widthmain, height - pfpside, pfpside, pfpside);

	private const int miniLabelWidth = 80;
	private const int labelIntrusionPixels = 0;
	private const int labelHeight = 60;
	private const int typeLabelWidth = 175;
	private const int hybridLabelWidth = 125;
	private const int labelMiniMargin = 10;
	private static readonly Rectangle rectLevelLabel = new(defMargin, defMargin, miniLabelWidth + labelIntrusionPixels, labelHeight);
	private static readonly Rectangle rectLevelText = new(defMargin + miniLabelWidth, defMargin, widthmain / 2 - defMargin - miniLabelWidth, labelHeight);

	private const int barMarginVertical = 9;
	private const int barMarginHorizontal = 125;
	private const int barHeight = 75 - 3 * barMarginVertical;

	internal class LevelRect
	{
		public Rectangle fullRect;
		public Func<float, Rectangle> Bar;
		public Rectangle currentLevel;
		public Rectangle nextLevel;
		public Rectangle typeLabel;
		public Rectangle rankLabel;
		public Rectangle rankText;
		public Rectangle expText;
		public LevelBarType leveltype;

		public LevelRect(int originHeight, LevelBarType leveltype = LevelBarType.Normal)
		{
			this.leveltype = leveltype;
			if (leveltype == LevelBarType.Normal)
			{
				fullRect = new Rectangle(defMargin, originHeight, levelWidth, levelHeight);
				Bar = (p) => new Rectangle(defMargin + barMarginHorizontal, originHeight + levelHeight - barHeight - barMarginVertical
					, (int)((levelWidth - 2 * barMarginHorizontal) * p), barHeight);
				currentLevel = new Rectangle(defMargin, originHeight + levelHeight - barHeight - barMarginVertical, barMarginHorizontal, barHeight + 2 * barMarginVertical);
				nextLevel = new Rectangle(levelWidth + defMargin - barMarginHorizontal, originHeight + levelHeight - barHeight - barMarginVertical, barMarginHorizontal, barHeight + 2 * barMarginVertical);
				typeLabel = new Rectangle(defMargin + labelMiniMargin, originHeight + labelMiniMargin, typeLabelWidth, labelHeight);
				rankLabel = new Rectangle(defMargin + typeLabelWidth, originHeight, miniLabelWidth + labelIntrusionPixels, labelHeight);
				rankText = new Rectangle(defMargin + miniLabelWidth + typeLabelWidth, originHeight, levelWidth * 2 / 3 - miniLabelWidth - typeLabelWidth - defMargin, labelHeight);
				expText = new Rectangle(levelWidth / 3, originHeight, levelWidth * 2 / 3, labelHeight);
			}
			else
			{
				fullRect = new Rectangle(leveltype == LevelBarType.HybridMain ? defMargin : widthmain / 2 + labelMiniMargin, originHeight,
					widthmain / 2 - labelMiniMargin - defMargin, levelHeight);
				Bar = (p) => new Rectangle(fullRect.X + barMarginHorizontal, originHeight + levelHeight - barHeight - barMarginVertical
					, (int)((fullRect.Width - barMarginHorizontal - labelMiniMargin) * p), barHeight);
				currentLevel = new Rectangle(fullRect.X, originHeight + levelHeight - barHeight - barMarginVertical, barMarginHorizontal, barHeight + 2 * barMarginVertical);
				nextLevel = default;
				typeLabel = new Rectangle(fullRect.X + labelMiniMargin, originHeight + labelMiniMargin, hybridLabelWidth, labelHeight);
				rankLabel = new Rectangle(fullRect.X, originHeight, fullRect.Width / 2 + labelIntrusionPixels, labelHeight);
				rankText = new Rectangle(fullRect.X + fullRect.Width / 2, originHeight, fullRect.Width / 2, labelHeight);
				expText = Bar(1);
			}
		}

		public enum LevelBarType
		{
			Normal,
			HybridMain,
			HybridSecondary
		}
	}

	internal class RankcardLevelData : LevelData
	{
		public bool isHybrid = false;
		public float Percent => Residualxp / (float)Levelxp;
		public string XpExpr => $"{Residualxp.ToUnit()} / {Levelxp.ToUnit()}{(isHybrid ? "" : " XP")}";
		public string xpType = "";

		public int rank;

		public LevelRect rects;

		public RankcardLevelData(long xp, LevelRect rects, GuildLevelConfig guildConfig, bool isHybrid = false) : base(xp, guildConfig)
		{
			this.rects = rects;
			this.isHybrid = isHybrid;
		}
	}

	public static async Task<Image> RenderRankCard(IUser user, CalculatedGuildUserLevel ul, UserRankcardConfig rankcardConfig, GuildUserLevelRepository levelsRepo, SettingsRepository configRepo)
	{
		var appconfig = await configRepo.GetAppSettings();
		var fontPath = IOPath.Join(appconfig.AbsolutePathToFileUpload, "Media", "Fonts", "rankcardfont.ttf");

		FontCollection fontCollection = new();
		fontCollection.Add(fontPath);
		var fontfamily = fontCollection.Families.First();

		Font fontTitle = new(fontfamily, 55);
		Font fontDefault = new(fontfamily, 40);
		Font fontMini = new(fontfamily, 22);

		List<RankcardLevelData> levelsData = new();
		int totallevel;

		string totallevelstr;

		var allUsers = levelsRepo.GetAllLevelsInGuild(ul.GuildId).ToList();
		allUsers.Sort((a, b) => b.TextXp.CompareTo(a.TextXp));
		var txtrank = allUsers.FindIndex(ul => ul.UserId == user.Id) + 1;
		allUsers.Sort((a, b) => b.VoiceXp.CompareTo(a.VoiceXp));
		var vcrank = allUsers.FindIndex(ul => ul.UserId == user.Id) + 1;

		levelsData.Add(new RankcardLevelData(ul.TotalXP, mainLevel, ul.Config!));

		allUsers.Sort((a, b) => b.TotalXP.CompareTo(a.TotalXP));
		levelsData[0].rank = allUsers.FindIndex(ul => ul.UserId == user.Id) + 1;

		levelsData[0].xpType = "Level";
		totallevel = levelsData[0].Level;
		totallevelstr = $"({ul.TotalXP.ToUnit()} XP)";

		if (rankcardConfig.RankcardFlags.HasFlag(RankcardFlags.ShowHybrid))
		{
			levelsData.Add(new RankcardLevelData(ul.TextXp > ul.VoiceXp ? ul.TextXp : ul.VoiceXp, mainHybridLevel, ul.Config!, true));
			levelsData.Add(new RankcardLevelData(ul.TextXp > ul.VoiceXp ? ul.VoiceXp : ul.TextXp, secondaryHybridLevel, ul.Config!, true));

			if (ul.TextXp > ul.VoiceXp)
			{
				levelsData[1].rank = txtrank;
				levelsData[1].xpType = "Txt";
				levelsData[2].rank = vcrank;
				levelsData[2].xpType = "VC";
			}
			else
			{
				levelsData[1].rank = vcrank;
				levelsData[1].xpType = "VC";
				levelsData[2].rank = txtrank;
				levelsData[2].xpType = "Txt";
			}
		}

		string BackgroundPath(string filename) => IOPath.Combine(appconfig.AbsolutePathToFileUpload, "Media", "Images", "Leveling", "Backgrounds", filename + ".png");
		Func<IImageProcessingContext, IImageProcessingContext> bgTransform;
		Func<IImageProcessingContext, IImageProcessingContext> pfpTransform;

		var xpColor = Graphics.ColorFromArgb(rankcardConfig.XpColor);
		var offColor = Graphics.ColorFromArgb(rankcardConfig.OffColor);
		var lvlBgColor = Graphics.ColorFromArgb(rankcardConfig.LevelBgColor);

		Image? bg = null;
		try
		{
			if (rankcardConfig.Background.StartsWith("http"))
				throw new FileLoadException();

			var fileName = BackgroundPath(rankcardConfig.Background ?? "default");
			bg = Image.Load(File.ReadAllBytes(fileName));
			bg.Mutate(ctx => ctx.Resize(mainRect.Size));
			bgTransform = g => g.DrawImage(bg, 1);
		}
		catch (FileNotFoundException)
		{
			Color bgc;
			if (Regex.IsMatch(rankcardConfig.Background, @"^(#|0x)?[0-9A-F]{6}$", RegexOptions.IgnoreCase))
			{
				bgc = Graphics.ColorFromArgb(0xff000000 | uint.Parse(rankcardConfig.Background[^6..], System.Globalization.NumberStyles.HexNumber));
			}
			else
			{
				bgc = Graphics.ColorFromArgb(0xff000000 | rankcardConfig.XpColor);
			}
			bgTransform = g => g.BackgroundColor(bgc);
		}
		catch (FileLoadException)
		{
			using HttpClient client = new();
			try
			{
				var req = new HttpRequestMessage(HttpMethod.Get, rankcardConfig.Background);
				//req.Headers.Add("Cookie", $"dexter_access_token=CfDJ8GovyKJyMhBEggK_51F19rNfPbYkIIjGhFyLj8l9ljgF5vQiGXaP9QQ-TCwPuLprmGuwukTA78KujaxbnceOlykINod-k2ZBeEeEltXu-iacTERy3FkYZRg_B2L-ufJwG8VsyR5Iwnja4ToQJiT93r_okr7F7FAbHwkHQ338QjXhs7NbTzXIy4aejSHHPbHbog4dc1abMlqTWx8d6AIX1CxDAuM4DcHhs_yNxk1oJiV21kOLOpCpq9aJKJNALKo6pjWQDuYhiZrXaHV742MiagZZRmDPkYUjppufIDDhYGgVbe7ECwV3DbX5yymXdz9UK5Nbl-q00TRD6wZxm-ddhf-IBnUTTNBv-9_FOPa17CKfSmscN7drS5i4D6PMfmCcZwTzooIpm5UOxhgQXH0AC3lSIjTl3OuwgZaXby9WqN7OG1dU__IavVUehCt9DmK75VseZtCE-uIu4i4UmzmNZPbGf8TlO7ac4Mgb0HwnXt_iUTXs7bzUGxU8_bQpWLLyWYJPlnVbNRTKYoBRJhxwwQ5NBL7yuTg3eAdPde3FGjmBNDC7NVwKsTWZ6Ps45tasR3dcuMeRwXS0pSADH879oIp0OK5oWIwhy4wa3tNJXKn95c3gFCL7MIxfE7dgPRmueM_Yb8EpjGTx8Gv6KkLdNoSzGtXaDOyu9YJ6mVUV__a0naETy9XPIABwveH7T-BO8ZL9N6Yw_RWtqKLzTjEnWH4TqB-gFgWWRwqjhIeNXW73-sw8zcVJjnkAQv4_VSqWYy7UJBy2ZO_F8VoZ01HbeC4h2hV9tMF36mip7UEv1b1yV0N-HoMIawxhDnqebYOScJVHnQlHdZE1zE1hsS2yyY5raPjQvNAVBtpvCex25ARnrVru0v0CzonCH2fpGxLbCqgqdPwH3wWseuEdOMRw1mK0PIQWVm6VFcn-1972h_75K3Wy3WyhYFCmxo7tfYQXwLuqPcodh8XOzrC9XLUJ4y0lONeX5M2N-e6PRmDZEV8VmVdlaRM4kM9B4s1vs4Mz-znKonidwhvGZSuRlJuIAx4");
				var resp = await client.SendAsync(req);
				if (!resp.IsSuccessStatusCode)
				{
					throw new HttpRequestException($"Received unsuccessful response to rankcard background image request.\n" +
						$"Code: {(int)resp.StatusCode} ({resp.ReasonPhrase})\n" +
						$"Headers: {string.Join("\n".PadRight("\nHeaders: ".Length), resp.Headers.Select(h => $"{h.Key}={string.Join(", ", h.Value)}"))}\n" +
						$"Content: {resp.Content}");
				}
				bg = Image.Load(await resp.Content.ReadAsByteArrayAsync());
				bg.Mutate(ctx => ctx.Resize(mainRect.Size));
				bgTransform = g => g.DrawImage(bg, 1);
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(e);
				Console.ForegroundColor = ConsoleColor.White;
				bgTransform = g => g.BackgroundColor(Graphics.ColorFromArgb(0xff000000 | rankcardConfig.XpColor));
			}
		}

		Image? pfp = null;
		using (HttpClient client = new())
		{
			try
			{
				var dataArr = await client.GetByteArrayAsync(user.GetAvatarOrDefaultUrl(size: 512));
				pfp = Image.Load(dataArr);
				pfp.Mutate(ctx => ctx.Resize(rectPfp.Size));

				if (rankcardConfig.RankcardFlags.HasFlag(RankcardFlags.ClipPfp))
				{
					pfpTransform = g => g.Clip(new PathBuilder().AddEllipticalArc(rectPfp, 0, 0, 360).Build(),
						clipg => clipg.DrawImage(pfp, new Point(rectPfp.Left, rectPfp.Top), 1)
					);
				}
				else
				{
					pfpTransform = g => g.DrawImage(pfp, new Point(rectPfp.Left, rectPfp.Top), 1);
				}
			}
			catch (HttpRequestException)
			{
				pfpTransform = g => g.Draw(new Pen(xpColor, 5), new EllipsePolygon(rectPfp.Left, rectPfp.Top, rectPfp.Width, rectPfp.Height));
			}
		}

		Image result = new Image<Rgba32>(widthmain + pfpside, height);
		result.Mutate(g =>
		{
			var offset = TextMeasurer.Measure(totallevel.ToString(), new TextOptions(fontTitle));
			const int margin = 5;

			g = bgTransform(g);

			g = g.Fill(lvlBgColor, titleRect)
			.DrawTextInRect("LEVEL", rectLevelLabel, fontMini, offColor, HorizontalAlignment.Right, VerticalAlignment.Bottom)
			.DrawTextInRect(totallevel.ToString(), rectLevelText, fontTitle, xpColor, HorizontalAlignment.Left, VerticalAlignment.Bottom)
			.DrawTextInRect(totallevelstr,
				new Rectangle(rectLevelText.X + (int)offset.Width + margin, rectLevelText.Y, widthmain / 2 - miniLabelWidth - margin - (int)offset.Width, labelHeight),
				fontDefault, xpColor, HorizontalAlignment.Left, VerticalAlignment.Bottom);
			
			g = g.DrawLevels(fontTitle, fontDefault, fontMini, levelsData, rankcardConfig);

			if (bg is not null)
				bg.Dispose();

			const int pfpmargin = 3;
			if (rankcardConfig.RankcardFlags.HasFlag(RankcardFlags.PfpBackground))
				g = pfpTransform(g.Fill(Graphics.ColorFromArgb(0xff3f3f3f)
					, new EllipsePolygon(rectPfp.X + rectPfp.Width / 2, rectPfp.Y + rectPfp.Height / 2, rectPfp.Width + 2 * pfpmargin, rectPfp.Height + 2 * pfpmargin)));

			if (pfp is not null)
				pfp.Dispose();

			g = g.DrawTextInRect($"{user.Username}#{user.Discriminator}", rectName, fontDefault, offColor,
				HorizontalAlignment.Right, VerticalAlignment.Center);
		});
		return result;
	}

	/*
	private readonly static float[] lineartransform = new float[] { 0, 0, 0, 0, 1 };
	public static ColorMatrix ToColorMatrix(this Color color)
	{
		return new ColorMatrix(new float[][] {
				new float[] {color.R / 255f, 0, 0, 0, 0},
				new float[] {0, color.G / 255f, 0, 0, 0},
				new float[] {0, 0, color.B / 255f, 0, 0},
				new float[] {0, 0, 0, color.A / 255f, 0},
				lineartransform
				});
	}
	*/

	private static IImageProcessingContext DrawLevels(this IImageProcessingContext g, Font fontTitle, Font fontDefault, Font fontMini, IEnumerable<RankcardLevelData> levels, UserRankcardConfig prefs)
	{
		var xpColor = Graphics.ColorFromArgb(prefs.XpColor);
		var offColor = Graphics.ColorFromArgb(prefs.OffColor);

		foreach (var ld in levels)
		{
			if (ld is null) continue;
			var barRect = ld.rects.Bar(1);
			var levelPath = Graphics.RoundedRect(ld.rects.fullRect, ld.rects.fullRect.Height / 3);
			var barWholePath = Graphics.RoundedRect(barRect, barRect.Height / 2);
			var barXPGPath = Graphics.RoundedRect(ld.rects.Bar(ld.Percent), barRect.Height / 2);
			var barInnerClipPath = Graphics.RoundedRect(new Rectangle(barRect.X + 2, barRect.Y + 2, barRect.Width - 4, barRect.Height - 4), barRect.Height / 2 - 2);
			var levelRenderArea = levelPath.Clip(barWholePath);

			g = g.Fill(Graphics.ColorFromArgb(0xe0000000), barWholePath)
			.Clip(barInnerClipPath,
				gclip => gclip.Fill(xpColor, barXPGPath))
			.Fill(Graphics.ColorFromArgb(prefs.LevelBgColor), levelRenderArea)
			.DrawTextInRect(ld.xpType, ld.rects.typeLabel, fontTitle, offColor, HorizontalAlignment.Center, VerticalAlignment.Center);
			
			if (ld.rects.rankLabel != default)
				g = g.DrawTextInRect("RANK", ld.rects.rankLabel, fontMini, offColor, HorizontalAlignment.Right, VerticalAlignment.Bottom);
			
			if (ld.rects.rankText != default)
				g = g.DrawTextInRect($"#{ld.rank}", ld.rects.rankText, fontTitle, offColor, HorizontalAlignment.Left, VerticalAlignment.Bottom);

			g = g.DrawTextInRect(ld.Level.ToString(), ld.rects.currentLevel, fontTitle, xpColor, HorizontalAlignment.Center, VerticalAlignment.Center);
			if (ld.rects.nextLevel != default)
				g = g.DrawTextInRect((ld.Level + 1).ToString(), ld.rects.nextLevel, fontTitle, xpColor, HorizontalAlignment.Center, VerticalAlignment.Center);

			var textXPTarget = ld.rects.expText;
			if (!ld.isHybrid && prefs.RankcardFlags.HasFlag(RankcardFlags.InsetMainXP))
			{
				ld.isHybrid = true;
				textXPTarget = barRect;
			}

			if (ld.isHybrid)
			{
				var overXPColor = xpColor.GetBrightness() < 0.5 ? Color.White : Color.Black;
				g = g.DrawTextInRect(ld.XpExpr, textXPTarget, fontDefault, xpColor, HorizontalAlignment.Center, VerticalAlignment.Bottom)
				.Clip(barXPGPath,
					gclip => gclip.DrawTextInRect(ld.XpExpr, textXPTarget, fontDefault, overXPColor, HorizontalAlignment.Center, VerticalAlignment.Bottom));
			}
			else
			{
				g = g.DrawTextInRect(ld.XpExpr, textXPTarget, fontDefault, xpColor, HorizontalAlignment.Right, VerticalAlignment.Bottom);
			}
		}

		return g;
	}
}
