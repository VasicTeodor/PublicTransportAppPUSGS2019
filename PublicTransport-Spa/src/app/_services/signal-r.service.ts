import { Injectable } from '@angular/core';
import { Location } from '../_models/location';
import * as signalR from '@aspnet/signalr';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {
public data: Location;

private hubConnection: signalR.HubConnection;

constructor() { }

public startConnection = () => {
  this.hubConnection = new signalR.HubConnectionBuilder()
                            .withUrl('http://localhost:5000/buslocation')
                            .build();
  this.hubConnection.start().then(() => console.log('Connection started'))
                            .catch(err => console.log('Error while starting connection: ' + err));
}

public addTransferBusLocationListener = () => {
  this.hubConnection.on('sendbuslocation', (data) => {
      this.data = data;
      console.log(data);
  });
}

}
