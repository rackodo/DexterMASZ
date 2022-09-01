import { ModCase } from "./ModCase";

export interface ModCaseComment {
    id: number;
    message: string;
    modCase?: ModCase;
    userId: string;
    createdAt: Date;
}
