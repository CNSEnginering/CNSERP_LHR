import { Component, Injector, ViewEncapsulation, ViewChild, EventEmitter, Output } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import { SalarySheetServiceProxy } from '../shared/services/salarySheet.service';
import { FileDownloadService } from '@shared/utils/file-download.service';
import { AttendanceServiceProxy } from '../shared/services/attendanceV2.service';
import * as moment from 'moment';
import { VoucherEntryServiceProxy } from '@shared/service-proxies/service-proxies';
import { CreateOrEditJVEntryModalComponent } from '@app/main/finance/GeneralLedger/transaction/jvEntry/create-or-edit-jvEntry-modal.component';

@Component({
    templateUrl: './salarySheet.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class SalarySheetComponent extends AppComponentBase {
    @ViewChild("createOrEditJVEntryModal", { static: true })
	createOrEditJVEntryModal: CreateOrEditJVEntryModalComponent;


    processing = false;
    processing1 = false;
    processing2 = false;

    // salaryYear = 'Select Year';
    // salaryMonth = 'Select Month';

    fromDate: Date = new Date();
    toDate: Date = new Date();

    yearAndMonth = new Date();
    salaryYear: number;
    salaryMonth: number;

    years: any;

    constructor(
        injector: Injector,
        private _salarySheetServiceProxy: SalarySheetServiceProxy,
        private _attendanceServiceProxy: AttendanceServiceProxy,
        private _fileDownloadService: FileDownloadService,
        private _voucherEntryServiceProxy: VoucherEntryServiceProxy,
    ) {
        super(injector);
    }

    ngOnInit(): void {
        //this.getYears()
    }

    onOpenCalendar(container) {
        container.monthSelectHandler = (event: any): void => {
            container._store.dispatch(container._actions.select(event.date));
        };
        container.setViewMode('month');
    }

    scheduleAttendance(): void {
        this.processing1 = true;

        this._attendanceServiceProxy.scheduleAttendance(moment(this.fromDate).startOf('day'), moment(this.toDate).startOf('day')).subscribe(result => {
            debugger
            if (result == "done") {
                this.processing1 = false;
                this.notify.info(this.l('Scheduled Successfully'));
            } else {
                this.processing1 = false;
                this.notify.error(this.l('Scheduled Failed'));
            }
        });
    }

    createGLTRHeader(BookID,BookName) {
		debugger;
        this.processing = true;
		if (this.yearAndMonth == null) {
            this.notify.error(this.l('Please Select Year and Month'));
            return;
        }
        this.salaryYear = this.yearAndMonth.getFullYear();
        this.salaryMonth = this.yearAndMonth.getMonth() + 1;

		var bookId = BookID;
		var bookName =BookName;

		
				this._voucherEntryServiceProxy
					.getMaxDocId(bookId
						, true, moment(new Date()).format("LLLL"))
					.subscribe(result => {
						debugger
						this.createOrEditJVEntryModal.JvForsalarySheetshow(
                            
							this.salaryYear,
							this.salaryMonth,
							result,
							bookId,
							bookName
						);
                        
					});
        this.processing = false;
	}

    markWholeMonthAttendance(): void {
        this.processing2 = true;

        this._attendanceServiceProxy.markWholeMonthAttendance(moment(this.fromDate).startOf('day'), moment(this.toDate).startOf('day')).subscribe(result => {
            debugger
            if (result == "done") {
                this.processing2 = false;
                this.notify.info(this.l('Attendance Marked Successfully'));
            } else {
                this.processing2 = false;
                this.notify.error(this.l('Attendance Marking Failed'));
            }
        });
    }

    

    processSalarySheet(): void {
        debugger;
        if (this.yearAndMonth == null) {
            this.notify.error(this.l('Please Select Year and Month'));
            return;
        }
        this.salaryYear = this.yearAndMonth.getFullYear();
        this.salaryMonth = this.yearAndMonth.getMonth() + 1;

        // if (this.salaryYear == 'Select Year') {
        //     this.notify.error(this.l('Please select Salary Year'));
        //     return;
        // }

        // if (this.salaryMonth == 'Select Month') {
        //     this.notify.error(this.l('Please select Salary Month'));
        //     return;
        // }
        this.message.confirm(
            'Salary Generation',
            (isConfirmed) => {
                if (isConfirmed) {
                    this.processing = true;
                    debugger;
                    this._salarySheetServiceProxy.processSalarySheet(Number(this.salaryMonth), Number(this.salaryYear)).subscribe(result => {
                        debugger
                        if (result == "done") {
                            this.processing = false;
                            this.notify.info(this.l('ProcessSuccessfully'));
                        } else {
                            this.processing = false;
                            this.notify.error(this.l('ProcessFailed'));
                        }
                    });
                }
            }
        );
    }
    exportToExcel(): void {
        // if (this.salaryYear == 'Select Year') {
        //     this.notify.error(this.l('Please select Salary Year'));
        //     return;
        // }

        // if (this.salaryMonth == 'Select Month') {
        //     this.notify.error(this.l('Please select Salary Month'));
        //     return;
        // }
        if (this.yearAndMonth == null) {
            this.notify.error(this.l('Please Select Salary Year and Month'));
            return;
        }
        this.salaryYear = this.yearAndMonth.getFullYear();
        this.salaryMonth = this.yearAndMonth.getMonth() + 1;

        this._salarySheetServiceProxy.getSalarySheetToExcel(Number(this.salaryMonth), Number(this.salaryYear)).subscribe(result => {
            this._fileDownloadService.downloadTempFile(result);
        });
    }

    // getYears() {
    //     this.years = new Date().getFullYear();
    //     var range = [];
    //     for (var i = -40; i <= 40; i++) {
    //         range.push(this.years + i);
    //     }
    //     this.years = range;
    // }

}

