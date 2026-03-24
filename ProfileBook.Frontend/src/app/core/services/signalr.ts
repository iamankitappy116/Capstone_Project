import { Injectable } from "@angular/core";
import * as signalR from "@microsoft/signalr";
import { Subject } from "rxjs";

@Injectable({ providedIn: "root" })
export class SignalRService {
  private hubConnection: signalR.HubConnection | null = null;
  notification$ = new Subject<string>();

  startConnection() {
    const token = sessionStorage.getItem("token");
    if(!token) return;

    this.hubConnection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5134/notificationHub", {
          accessTokenFactory: () => token
        })
        .withAutomaticReconnect()
        .build();

    this.hubConnection
      .start()
      .then(() => console.log("SignalR Connected"))
      .catch(err => console.error("SignalR Connection Error: ", err));

    this.hubConnection.on("ReceivedNotification", (message: string) => {
      this.notification$.next(message);
    });
  }
  stopConnection() {
    this.hubConnection?.stop();
  } 
}