import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { PaymentDetailService } from '../../core/services/payment-detail.service';
import { PaymentDetail } from '../../core/models/payment-detail.model';
import { PaymentDetailFormComponent } from "./payment-detail-form/payment-detail-form.component";

@Component({
  selector: 'app-payment-details',
  templateUrl: './payment-details.component.html',
  standalone: true,
  imports: [PaymentDetailFormComponent]
})
export class PaymentDetailsComponent implements OnInit {

  constructor(public service: PaymentDetailService, private toastr: ToastrService) {
  }

  ngOnInit(): void {
    console.log('PaymentDetailsComponent initialized');
    this.service.refreshList();
  }

  populateForm(selectedRecord: PaymentDetail) {
   this.service.formData = Object.assign({}, selectedRecord);
  }

   onDelete(id: any) {
    if (confirm('Are you sure to delete this record?'))
      this.service.deletePaymentDetail(id)
        .subscribe({
          next: res => {
            this.service.list = res as PaymentDetail[]
            this.toastr.error('Deleted successfully', 'Payment Detail Register', {
              toastClass: 'custom-toastr',
              timeOut: 1000
            });
            this.service.refreshList();
          },
          error: err => { console.log(err) }
        })
   }

}