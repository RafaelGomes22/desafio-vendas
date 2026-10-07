import { Component } from '@angular/core';
import { Venda } from './core/models/venda.model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  vendas: Venda[] = [];
}
