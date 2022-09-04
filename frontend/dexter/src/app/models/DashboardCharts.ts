import { DbCount } from "./DbCount";

export interface DashboardCharts {
  modCases: DbCount[];
  punishments: DbCount[];
  autoMods: DbCount[];
}
