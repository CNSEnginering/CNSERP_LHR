import { Component, Injector, ViewEncapsulation, ViewChild, EventEmitter, Output } from '@angular/core';
import { Router } from '@angular/router';
import { AppComponentBase } from '@shared/common/app-component-base';
import { appModuleAnimation } from '@shared/animations/routerTransition';
import * as _ from 'lodash';
import * as moment from 'moment';
import { FiscalDateService } from '@app/shared/services/fiscalDate.service';
import { ReportviewrModalComponent } from '@app/shared/common/reportviewr-modal/reportviewr-modal.component';
@Component({
    templateUrl: './setupReports.component.html',
    encapsulation: ViewEncapsulation.None,
    animations: [appModuleAnimation()]
})
export class SetupReportsComponent extends AppComponentBase {
    @ViewChild('reportView', { static: true }) reportView: ReportviewrModalComponent;


    fromCode = 0;
    toCode = 999999;
    description = "";

    documentListing: string;

    yearAndMonth = new Date();
    salaryYear: number;
    salaryMonth: number;

    constructor(
        injector: Injector,
        private route: Router
    ) {
        super(injector);
    }

    ngOnInit() {

    }

    onOpenCalendar(container) {
        debugger;
        container.monthSelectHandler = (event: any): void => {
            container._store.dispatch(container._actions.select(event.date));
        };
        container.setViewMode('month');
    }

    getReport() {
        switch (this.documentListing) {
            case 'designationListing':
                this.processReport("Designation");
                break;
            case 'departmentListing':
                this.processReport("Department");
                break;
            case 'educationListing':
                this.processReport("Education");
                break;
            case 'locationListing':
                this.processReport("Location");
                break;
            case 'religionListing':
                this.processReport("Religion");
                break;
            case 'empTypeListing':
                this.processReport("EmployeeType");
                break;
            case 'shiftListing':
                this.processReport("Shift");
                break;
            case 'sectionListing':
                this.processReport("Section");
                break;
            case 'gradeListing':
                this.processReport("Grade");
                break;
            default:
                break;
        }
    }

    processReport(report: string) {
        debugger;
        // if (this.yearAndMonth == null) {
        //     this.notify.error(this.l('Please Select Year and Month'));
        //     return;
        // }
        // this.salaryYear = this.yearAndMonth.getFullYear();
        // this.salaryMonth = this.yearAndMonth.getMonth() + 1;

        let repParams = '';

        switch (report) {
            case 'Designation':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                if (this.description !== undefined)
                    repParams += encodeURIComponent("" + this.description) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'Department':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                if (this.description !== undefined)
                    repParams += encodeURIComponent("" + this.description) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'Education':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                if (this.description !== undefined)
                    repParams += encodeURIComponent("" + this.description) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'Location':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                if (this.description !== undefined)
                    repParams += encodeURIComponent("" + this.description) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'Religion':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                if (this.description !== undefined)
                    repParams += encodeURIComponent("" + this.description) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'EmployeeType':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                if (this.description !== undefined)
                    repParams += encodeURIComponent("" + this.description) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'Shift':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                if (this.description !== undefined)
                    repParams += encodeURIComponent("" + this.description) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'Section':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                if (this.description !== undefined)
                    repParams += encodeURIComponent("" + this.description) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            case 'Grade':
                if (this.fromCode !== undefined)
                    repParams += encodeURIComponent("" + this.fromCode) + "$";
                if (this.toCode !== undefined)
                    repParams += encodeURIComponent("" + this.toCode) + "$";
                if (this.description !== undefined)
                    repParams += encodeURIComponent("" + this.description) + "$";
                repParams = repParams.replace(/[?$]&/, "");
                break;
            default:
                break;
        }
        this.reportView.show(report, repParams);
    }


}

