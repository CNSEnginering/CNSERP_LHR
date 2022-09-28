import { Component, ViewChild, Injector, Output, EventEmitter } from '@angular/core';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { TaxClassesServiceProxy, CreateOrEditTaxClassDto } from '@shared/service-proxies/service-proxies';
import { AppComponentBase } from '@shared/common/app-component-base';
import * as moment from 'moment';
import { CommonServiceLookupTableModalComponent } from '@app/finders/commonService/commonService-lookup-table-modal.component';
import { FinanceLookupTableModalComponent } from '@app/finders/finance/finance-lookup-table-modal.component';


@Component({
	selector: 'createOrEditTaxClassModal',
	templateUrl: './create-or-edit-taxClass-modal.component.html'
})
export class CreateOrEditTaxClassModalComponent extends AppComponentBase {

	@ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
	@ViewChild('commonServiceLookupTableModal', { static: true }) commonServiceLookupTableModal: CommonServiceLookupTableModalComponent;
	@ViewChild('FinanceLookupTableModal', { static: true }) FinanceLookupTableModal: FinanceLookupTableModalComponent;

	@Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
	classtype:any;
	active = false;
	saving = false;

	TAXAUTHDESC = "";
	classTypes = [];

	taxClass: CreateOrEditTaxClassDto = new CreateOrEditTaxClassDto();

	audtdate: Date;
	taxAuthorityTAXAUTH = '';
	target: string;
	taxaccdesc: string;
	taxClassExists: Boolean=false;

	constructor(
		injector: Injector,
		private _taxClassesServiceProxy: TaxClassesServiceProxy
	) {
		super(injector);
	}

	show(taxClassId?: number): void {
		debugger;
		this.audtdate = null;
		this.classTypes=[];
		if (!taxClassId) {
			this.taxClass = new CreateOrEditTaxClassDto();
			this.taxClass.id = taxClassId;
			this.taxAuthorityTAXAUTH = '';
			this.taxClass.isActive = true;
			this.taxaccdesc = "";
			this.active = true;
			this.modal.show();
		} else {
			this._taxClassesServiceProxy.getTaxClassForEdit(taxClassId).subscribe(result => {
				debugger;
				this.classtype =result.taxClass.classtype==null?0:parseInt(result.taxClass.classtype);
				this.taxClass = result.taxClass;
				this.taxaccdesc = result.taxAccDesc;
				if (this.taxClass.audtdate) {
					this.audtdate = this.taxClass.audtdate.toDate();
				}
				this.taxAuthorityTAXAUTH = result.taxAuthorityTAXAUTH;

				this.filterClassType(result.taxClass.transtype);
				if(this.classtype>0)
					this.taxClass.classtype = this.classtype;
				this.active = true;
				this.modal.show();
			});
		}
	}

	// numberOnly(event): boolean {
	//     debugger
	//     const charCode = (event.which) ? event.which : event.keyCode;
	//     if (charCode > 31 && (charCode < 48 || charCode > 57)) {
	//       return false;
	//     }
	//     return true;
	//   }

	save(): void {
		this.saving = true;

		this.taxClass.audtdate = moment();
		this.taxClass.audtuser = this.appSession.user.userName;
		this._taxClassesServiceProxy.createOrEdit(this.taxClass)
			.pipe(finalize(() => { this.saving = false; }))
			.subscribe(() => {
				this.notify.info(this.l('SavedSuccessfully'));
				this.close();
				this.modalSave.emit(null);
			});
	}

	openSelectTaxAuthorityModal() {
		debugger;
		this.commonServiceLookupTableModal.id = this.taxClass.taxauth;
		this.commonServiceLookupTableModal.displayName = this.taxAuthorityTAXAUTH;
		this.commonServiceLookupTableModal.show("TaxAuthority");
	}


	setTaxAuthorityIdNull() {
		this.taxClass.taxauth = '';
		this.taxAuthorityTAXAUTH = '';
	}


	getNewTaxAuthorityId() {
		debugger;
		this.taxClass.taxauth = this.commonServiceLookupTableModal.id;
		this.taxAuthorityTAXAUTH = this.commonServiceLookupTableModal.displayName;

		this.getMaxTaxClassId(this.taxClass.taxauth);
	}


	filterClassType(val: number) {
		debugger;
		// this.taxClass.classtype='';
		if (val == 1) {
			this.classTypes = [{ name: "Customers", val: 1 }, { name: "Items", val: 2 }];
			this.taxClass.classtype=this.classTypes[0].val;
		}
		else if (val == 2) {
			this.classTypes = [{ name: "Venders", val: 1 }, { name: "Items", val: 2 }];
			this.taxClass.classtype=this.classTypes[0].val;
		}
		else {
			this.classTypes = [];
			this.classTypes = [{ name: "Customers", val: 1 }, { name: "Venders", val: 2 }];
			this.taxClass.classtype=this.classTypes[0].val;
		}

        /*if(!this.taxClass.id)
            this.getMaxTaxClassId();*/
	}

	getMaxTaxClassId(authId) {
		debugger;
		this._taxClassesServiceProxy.getMaxTaxClassId(authId)
			.subscribe(result => {
				debugger;
				if (result != 0) {
					this.taxClass.classid = result + 1;
				} else {
					this.taxClass.classid = 1;
				}
			})
	}

	getNewFinanceModal() {
		debugger;
		switch (this.target) {
			case "ChartOfAccount":
				this.getNewChartOfAC();
				break;
			default:
				break;
		}
	}

	//=====================Chart of Ac Model================
	openSelectChartofACModal() {
		this.target = "ChartOfAccount";
		this.FinanceLookupTableModal.id = this.taxClass.taxaccid;
		this.FinanceLookupTableModal.displayName = this.taxaccdesc;
		this.FinanceLookupTableModal.show(this.target);
	}

	checkTaxClassExists() {
		this._taxClassesServiceProxy.getTaxClassExists(this.FinanceLookupTableModal.id, this.taxClass.taxauth).subscribe
			(
				data => {
					this.taxClassExists = data["result"];
					if (this.taxClassExists)
						this.notify.warn("Tax class with this account already exists");
				}
			);
	}

	setAccountIDNull() {
		this.taxClass.taxaccid = '';
		this.taxaccdesc = '';
	}

	getNewChartOfAC() {
		debugger;
		this.taxClass.taxaccid = this.FinanceLookupTableModal.id;
		this.taxaccdesc = this.FinanceLookupTableModal.displayName;
		//this.checkTaxClassExists();
	}
	//=====================Chart of Ac Model================

	close(): void {

		this.active = false;
		this.modal.hide();
	}
}
