export interface ServiceStatus {
    online: boolean;
    lastDisconnect?: Date;
    responseTime: number;
    message?: string;
}
