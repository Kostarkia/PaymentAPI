import { Component } from '@angular/core';
import { PaymentDetailsComponent } from "./pages/payment-details/payment-details.component";

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [PaymentDetailsComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'PaymentApp';
}
