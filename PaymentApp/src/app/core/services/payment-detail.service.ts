import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { NgForm } from "@angular/forms";
import { PaymentDetail } from '../models/payment-detail.model';
import { environment } from '../../../environments/environment.development';
@Injectable({
  providedIn: 'root'
})
export class PaymentDetailService {

  url: string = environment.apiBaseUrl + '/PaymentDetail'
  list: PaymentDetail[] = [];
  formData: PaymentDetail = new PaymentDetail()
  formSubmitted: boolean = false;
  constructor(private http: HttpClient) { }

  refreshList() {
    console.log('PaymentDetailService initialized');
    this.http.get(this.url)
      .subscribe({
        next: res => {
          this.list = res as PaymentDetail[]
          console.log(" refresh " +this.list.values);
        },
        error: err => { console.log(err) }
      })
  }

  postPaymentDetail() {
    console.log('PaymentDetailService Tested : ', this.formData);
    var result = this.http.post(this.url, this.formData);
    return result;
  }

  putPaymentDetail() {
    return this.http.put(this.url + '/' + this.formData.paymentDetailID, this.formData)
  }


  deletePaymentDetail(id: any) {
    return this.http.delete(`${this.url}/${id}`);
  }


  resetForm() {
    this.formData = new PaymentDetail()
    this.formSubmitted = false;
  }
}