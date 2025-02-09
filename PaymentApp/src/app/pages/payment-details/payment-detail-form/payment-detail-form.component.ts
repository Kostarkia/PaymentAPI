import { Component } from '@angular/core';
import { FormBuilder, FormGroup, NgForm, Validators } from "@angular/forms";
import { ToastrService } from 'ngx-toastr';
import { PaymentDetailService } from '../../../core/services/payment-detail.service';
import { PaymentDetail } from '../../../core/models/payment-detail.model';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-payment-detail-form',
  templateUrl: './payment-detail-form.component.html',
  imports: [ReactiveFormsModule, CommonModule],
  styles: [
  ],
})
export class PaymentDetailFormComponent {
  paymentForm!: FormGroup;


  constructor(public service: PaymentDetailService, private toastr: ToastrService, private fb: FormBuilder) {
  }

  ngOnInit(): void {
    this.paymentForm = this.fb.group({
      paymentDetailID: [null],
      cardOwnerName: ['', Validators.required],
      cardNumber: ['', [Validators.required, Validators.minLength(16), Validators.maxLength(16)]],
      securityCode: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(3)]],
      expirationDate: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(5)]],
    });

    if (!this.service.formData) {
      this.service.formData = new PaymentDetail();
    }
  }

  insertRecord() {
    this.service.postPaymentDetail()
      .subscribe({
        next: res => {
          this.service.list = res as PaymentDetail[]
          this.toastr.success('Inserted successfully!', 'Payment Detail Register', {
            toastClass: 'custom-toastr',
            timeOut: 1000
          });
          
          this.service.refreshList();
          this.paymentForm.reset();
        },
        error: err => { console.log(err) }
      })
  }
  updateRecord() {
    this.service.putPaymentDetail()
      .subscribe({
        next: res => {
          this.service.list = res as PaymentDetail[]
          this.paymentForm.reset();
          this.toastr.info('Updated successfully', 'Payment Detail Register')
          this.service.refreshList();
        },
        error: err => { console.log(err) }
      })
  }
 
  onSubmit() {
    this.service.formSubmitted = true

    if (this.paymentForm.valid) {
      this.service.formData = this.paymentForm.value;

      if (this.service.formData.paymentDetailID == null)
        this.insertRecord()
      else
        this.updateRecord()
    }

  }
}