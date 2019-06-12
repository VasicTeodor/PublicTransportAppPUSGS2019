import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-viewLines',
  templateUrl: './viewLines.component.html',
  styleUrls: ['./viewLines.component.css']
})
export class ViewLinesComponent implements OnInit {
  isCollapsedStations = true;
  isCollapsedBuses = true;

  constructor() { }

  ngOnInit() {
  }

}
