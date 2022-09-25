using Bot.Data;
using Bot.Extensions;
using Discord;
using Levels.Data;
using Levels.Enums;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Text.RegularExpressions;
using Color = SixLabors.ImageSharp.Color;
using Image = SixLabors.ImageSharp.Image;
using IOPath = System.IO.Path;
using Path = System.IO.Path;

namespace Levels.Models;

public static partial class Rankcard
{
	public static readonly Size RankCardSize = new(widthmain + pfpside, height);

	private const int widthmain = 1000;
	private const int height = 450;
	private const int pfpside = 350;
	private const int normalMargin = 25;
	private const int levelHeight = (pfpside - 3 * normalMargin) / 2;
	private const int maxTitleXpWidth = widthmain / 2;

	private static readonly Size titleSize = new(RankCardSize.Width - 2 * normalMargin, labelHeight);
	private static Rectangle TitleRect(UserRankcardConfig config) => new(config.TitleOffset + normalMargin, titleSize);
	private static readonly Size levelsSize = new(widthmain - 2 * normalMargin, pfpside - 2 * normalMargin);
	private static Rectangle LevelsRect(UserRankcardConfig config) => new(config.LevelOffset + normalMargin, levelsSize);
	private static int PfpMargin(UserRankcardConfig config) => (int)(pfpside * (1 - (config.PfpRadiusFactor < 0.1 ? 0.1 : config.PfpRadiusFactor))) / 2;
	private static Size PfpSize(UserRankcardConfig config) => new(pfpside - 2 * PfpMargin(config));
	private static Rectangle PfpRect(UserRankcardConfig config) => new(config.PfpOffset + PfpMargin(config), PfpSize(config));

	private static LevelRect MainLevel(Rectangle container) => new(container, LevelRect.LevelBarType.Normal);
	private static LevelRect MainHybridLevel(Rectangle container) => new(container, LevelRect.LevelBarType.HybridMain);
	private static LevelRect SecondaryHybridLevel(Rectangle container) => new(container, LevelRect.LevelBarType.HybridSecondary);

	private const int miniLabelWidth = 80;
	private const int labelIntrusionPixels = 0;
	private const int labelHeight = 60;
	private const int typeLabelWidth = 175;
	private const int labelMiniMargin = 10;
	private const int labelBaselineDeltaTitle = 8;
	private const int labelBaselineDeltaNormal = 3;

	private static Rectangle LevelLabelRect(UserRankcardConfig config) => new(config.TitleOffsetX + normalMargin, config.TitleOffsetY + normalMargin, miniLabelWidth + labelIntrusionPixels, labelHeight);
	private static Rectangle LevelTextRect(UserRankcardConfig config) => new(config.TitleOffsetX + miniLabelWidth + normalMargin, config.TitleOffsetY + normalMargin, maxTitleXpWidth - miniLabelWidth, labelHeight + labelBaselineDeltaTitle);
	private static Rectangle NameRect(UserRankcardConfig config) => new(config.TitleOffsetX + normalMargin, config.TitleOffsetY + normalMargin, RankCardSize.Width - 2 * normalMargin, labelHeight + labelBaselineDeltaNormal);

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

		public LevelRect(Rectangle container, LevelBarType leveltype = LevelBarType.Normal)
		{
			var originX = container.X;
			var originY = container.Y;
			int width;
			int barwidth;

			this.leveltype = leveltype;
			if (leveltype == LevelBarType.Normal)
			{
				width = container.Width;
				barwidth = width - 2 * barMarginHorizontal;

				nextLevel = new Rectangle(originX + width - barMarginHorizontal, originY + levelHeight - barHeight - barMarginVertical, barMarginHorizontal, barHeight + 2 * barMarginVertical);
			}
			else
			{
				width = (container.Width - normalMargin) / 2;
				if (leveltype == LevelBarType.HybridSecondary)
					originX += width + normalMargin;
				originY += (container.Height + normalMargin) / 2;
				barwidth = width - barMarginHorizontal - labelMiniMargin;

				nextLevel = default;
			}

			fullRect = new Rectangle(originX, originY, width, levelHeight);

			Bar = (p) => new Rectangle(originX + barMarginHorizontal, originY + levelHeight - barHeight - barMarginVertical
			, (int)(barwidth * p), barHeight);
			currentLevel = new Rectangle(originX, originY + levelHeight - barHeight - barMarginVertical, barMarginHorizontal, barHeight + 2 * barMarginVertical);
			typeLabel = new Rectangle(originX + labelMiniMargin, originY + labelMiniMargin, typeLabelWidth, labelHeight);
			rankLabel = new Rectangle(originX + typeLabelWidth, originY, miniLabelWidth + labelIntrusionPixels, labelHeight);
			rankText = new Rectangle(originX + miniLabelWidth + typeLabelWidth, originY, width * 2 / 3 - miniLabelWidth - typeLabelWidth - normalMargin, labelHeight + labelBaselineDeltaTitle);
			expText = leveltype == LevelBarType.Normal ?
				new Rectangle(originX + width / 3, originY, width * 2 / 3 - labelMiniMargin, labelHeight) :
				Bar(1);
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
		var fontPath = IOPath.Join(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Fonts", "rankcardfont.ttf");

		FontCollection fontCollection = new();
		fontCollection.Add(fontPath);
		var fontfamily = fontCollection.Families.First();

		Font fontTitle = new(fontfamily, 55);
		Font fontDefault = new(fontfamily, 40);
		Font fontMini = new(fontfamily, 22);

		List<RankcardLevelData> levelsData = new();
		int totallevel;
		var totalxp = ul.TotalXP;

		string totallevelstr;

		var allUsers = levelsRepo.GetAllLevelsInGuild(ul.GuildId).ToList();
		var rank = 1;
		var txtrank = 1;
		var vcrank = 1;
		allUsers.ForEach(u =>
		{
			if (u.TotalXP > totalxp)
				rank++;
			if (u.TextXp > ul.TextXp)
				txtrank++;
			if (u.VoiceXp > ul.VoiceXp)
				vcrank++;
		});

		var levelsContainer = LevelsRect(rankcardConfig);
		levelsData.Add(new RankcardLevelData(ul.TotalXP, MainLevel(levelsContainer), ul.Config!));

		levelsData[0].rank = rank;
		levelsData[0].xpType = "Level";
		totallevel = levelsData[0].Level;
		totallevelstr = $"({ul.TotalXP.ToUnit()} XP)";

		if (rankcardConfig.RankcardFlags.HasFlag(RankcardFlags.ShowHybrid))
		{
			levelsData.Add(new RankcardLevelData(ul.TextXp > ul.VoiceXp ? ul.TextXp : ul.VoiceXp, MainHybridLevel(levelsContainer), ul.Config!, true));
			levelsData.Add(new RankcardLevelData(ul.TextXp > ul.VoiceXp ? ul.VoiceXp : ul.TextXp, SecondaryHybridLevel(levelsContainer), ul.Config!, true));

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

		Func<IImageProcessingContext, IImageProcessingContext> bgTransform;
		Func<IImageProcessingContext, IImageProcessingContext> pfpTransform;

		var xpColor = rankcardConfig.XpColor.ColorFromArgb();
		var offColor = rankcardConfig.OffColor.ColorFromArgb();
		var lvlBgColor = rankcardConfig.LevelBgColor.ColorFromArgb();

		Image bg = null;
		try
		{
			if (rankcardConfig.Background.StartsWith("http"))
				throw new FileLoadException();

			var fileName = Path.Join(LevelsImageRepository.GetDefaultBackgroundDir(), (rankcardConfig.Background ?? "default") + ".png");
			bg = Image.Load(File.ReadAllBytes(fileName));
			bg.Mutate(ctx => ctx.Resize(RankCardSize));
			bgTransform = g => g.DrawImage(bg, 1);
		}
		catch (FileNotFoundException)
		{
			Color bgc;
			if (Regex.IsMatch(rankcardConfig.Background, @"^(#|0x)?[0-9A-F]{6}$", RegexOptions.IgnoreCase))
			{
				bgc = (0xff000000 | uint.Parse(rankcardConfig.Background[^6..], System.Globalization.NumberStyles.HexNumber)).ColorFromArgb();
			}
			else
			{
				bgc = (0xff000000 | rankcardConfig.XpColor).ColorFromArgb();
			}
			bgTransform = g => g.BackgroundColor(bgc);
		}
		catch (FileLoadException)
		{
			using HttpClient client = new();

			Console.WriteLine(rankcardConfig.Background);
			try
			{
				var req = new HttpRequestMessage(HttpMethod.Get, rankcardConfig.Background);
				var resp = await client.SendAsync(req);
				if (!resp.IsSuccessStatusCode)
				{
					throw new HttpRequestException($"Received unsuccessful response to rankcard background image request.\n" +
						$"Code: {(int)resp.StatusCode} ({resp.ReasonPhrase})\n" +
						$"Headers: {string.Join("\n".PadRight("\nHeaders: ".Length), resp.Headers.Select(h => $"{h.Key}={string.Join(", ", h.Value)}"))}\n" +
						$"Content: {resp.Content}");
				}
				bg = Image.Load(await resp.Content.ReadAsByteArrayAsync());
				bg.Mutate(ctx => ctx.Resize(RankCardSize));
				bgTransform = g => g.DrawImage(bg, 1);
			}
			catch (Exception e)
			{
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.WriteLine(e);
				Console.ForegroundColor = ConsoleColor.White;
				bgTransform = g => g.BackgroundColor((0xff000000 | rankcardConfig.XpColor).ColorFromArgb());
			}
		}
		var rectPfp = PfpRect(rankcardConfig);

		Image pfp = null;
		using (HttpClient client = new())
		{
			try
			{
				var dataArr = await client.GetByteArrayAsync(user.GetAvatarOrDefaultUrl(size: 512));
				pfp = Image.Load(dataArr);
				pfp.Mutate(ctx => ctx.Resize(rectPfp.Size));

				if (rankcardConfig.RankcardFlags.HasFlag(RankcardFlags.ClipPfp))
				{
					pfpTransform = g => g.Clip(new PathBuilder().AddArc(rectPfp, 0, 0, 360).Build(),
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

		var rectLevelLabel = LevelLabelRect(rankcardConfig);
		var rectLevelText = LevelTextRect(rankcardConfig);

		Image result = new Image<Rgba32>(RankCardSize.Width, RankCardSize.Height);
		result.Mutate(g =>
		{
			var offset = TextMeasurer.Measure(totallevel.ToString(), new TextOptions(fontTitle));
			const int margin = 5;

			g = bgTransform(g);

			g = g.Fill(lvlBgColor, TitleRect(rankcardConfig))
			.DrawTextInRect("LEVEL", rectLevelLabel, fontMini, offColor, HorizontalAlignment.Right, VerticalAlignment.Bottom)
			.DrawTextInRect(totallevel.ToString(), rectLevelText, fontTitle, xpColor, HorizontalAlignment.Left, VerticalAlignment.Bottom)
			.DrawTextInRect(totallevelstr,
				new Rectangle(rectLevelText.X + (int)offset.Width + margin, rectLevelText.Y, widthmain / 2 - miniLabelWidth - margin - (int)offset.Width, labelHeight + labelBaselineDeltaNormal),
				fontDefault, xpColor, HorizontalAlignment.Left, VerticalAlignment.Bottom);

			g = g.DrawLevels(fontTitle, fontDefault, fontMini, levelsData, rankcardConfig);

			bg?.Dispose();

			const int pfpmargin = 3;
			if (rankcardConfig.RankcardFlags.HasFlag(RankcardFlags.PfpBackground))
				g = g.Fill(0xff3f3f3f.ColorFromArgb()
					, new EllipsePolygon(rectPfp.X + rectPfp.Width / 2, rectPfp.Y + rectPfp.Height / 2, rectPfp.Width + 2 * pfpmargin, rectPfp.Height + 2 * pfpmargin));

			if (rankcardConfig.RankcardFlags.HasFlag(RankcardFlags.DisplayPfp))
				g = pfpTransform(g);

			pfp?.Dispose();

			g = g.DrawTextInRect($"{user.Username}#{user.Discriminator}", NameRect(rankcardConfig), fontDefault, offColor,
				HorizontalAlignment.Right, VerticalAlignment.Bottom);
		});
		return result;
	}

	private static IImageProcessingContext DrawLevels(this IImageProcessingContext g, Font fontTitle, Font fontDefault, Font fontMini, IEnumerable<RankcardLevelData> levels, UserRankcardConfig prefs)
	{
		var xpColor = prefs.XpColor.ColorFromArgb();
		var offColor = prefs.OffColor.ColorFromArgb();

		foreach (var ld in levels)
		{
			if (ld is null) continue;
			var barRect = ld.rects.Bar(1);
			var levelPath = Graphics.RoundedRect(ld.rects.fullRect, ld.rects.fullRect.Height / 3);
			var barWholePath = Graphics.RoundedRect(barRect, barRect.Height / 2);
			var barXPGPath = Graphics.RoundedRect(ld.rects.Bar(ld.Percent), barRect.Height / 2);
			var barInnerClipPath = Graphics.RoundedRect(new Rectangle(barRect.X + 2, barRect.Y + 2, barRect.Width - 4, barRect.Height - 4), barRect.Height / 2 - 2);
			var levelRenderArea = levelPath.Clip(barWholePath);

			g = g.Fill(0xe0000000.ColorFromArgb(), barWholePath)
			.Clip(barInnerClipPath,
				gclip => gclip.Fill(xpColor, barXPGPath))
			.Fill(prefs.LevelBgColor.ColorFromArgb(), levelRenderArea)
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
