export interface UserSocialProfile {
  id: string;

  gender?: string
  sexuality?: string
  sonaInfo?: string
  nationality?: string
  languages?: string
  miscInfo?: string
  birthday?: {
    day: number,
    month: number,
    year: number
  }
  timezone?: {

  }
  preferences: SocialPreferences
}

export enum SocialPreferences {

}
