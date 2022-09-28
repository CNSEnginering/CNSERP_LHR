import { Component, ViewChild, Output, EventEmitter, Injector, OnInit, AfterViewInit } from '@angular/core';
import { AppComponentBase } from '@shared/common/app-component-base';
import { IcSegment2FinderModalComponent } from '../../FinderModals/ic-segment2-finder-modal.component';
import { IcSegmentFinderModalComponent } from '../../FinderModals/ic-segment-finder-modal.component';
import { ModalDirective } from 'ngx-bootstrap';
import { finalize } from 'rxjs/operators';
import { IcSegment3FinderModalComponent } from '../../FinderModals/ic-segment3-finder-modal.component';
import { CreateOrEditICItemDto, IcItemServiceProxy, UpdateItemPictureInput } from '../../shared/services/ic-Item.service';

import { ChartofAccountFinderComponent } from '@app/finders/finance/chartofAccount-finder/chartofAccount-finder.component';
import { AccountSubLedgersServiceProxy, ProfileServiceProxy, NameValueDto, ComboboxItemDto, throwException, AccountSubLedgerChartofControlLookupTableDto } from '@shared/service-proxies/service-proxies';
import { taxAuthorityFinderModalComponent } from '@app/finders/finance/taxAuthority-Finder/taxAuthority-Finder.component';
import { Opt4LookupTableModalComponent } from '../../FinderModals/opt4-lookup-table-modal.component';
import { Opt5LookupTableModalComponent } from '../../FinderModals/opt5-lookup-table-modal.component';
import * as moment from 'moment';
import { ICSetupsService } from '../../shared/services/ic-setup.service';
import { ICUOMsService } from '../../shared/services/ic-uoms.service';
import { IC_UNITDto } from '../../shared/services/ic-units-service';
import { ImageCroppedEvent } from 'ngx-image-cropper';
import { FileUploader, FileUploaderOptions, FileItem } from 'ng2-file-upload';
import { AppConsts } from '@shared/AppConsts';
import { IAjaxResponse } from 'abp-ng2-module/dist/src/abpHttpInterceptor';
import { TokenService } from 'abp-ng2-module/dist/src/auth/token.service';
import { Dimensions } from 'ngx-image-cropper/src/interfaces';
import { PriceListService } from '../../shared/services/priceList.service';
//import { IcItemPrictureModalComponent } from './ic-item-pricture-modal.component';


@Component({
    selector: 'createOrEditIcitemModal',
    templateUrl: './create-or-edit-ic-item-modal.component.html',
    styles: [`.user-edit-dialog-profile-image {
    margin-bottom: 20px;
        }`
    ]
})
export class CreateOrEditIcItemModalComponent extends AppComponentBase  {

    @ViewChild('ICSegment1FinderModal', { static: true }) icSegment1FinderModal: IcSegmentFinderModalComponent;
    @ViewChild('ICSegment2FinderModal', { static: true }) icSegment2FinderModal: IcSegment2FinderModalComponent;
    @ViewChild('ICSegment3FinderModal', { static: true }) icSegment3FinderModal: IcSegment3FinderModalComponent;
    @ViewChild('TaxAuthorityFinderModal', { static: true }) taxAuthorityFinderModal: taxAuthorityFinderModalComponent;
    @ViewChild('ChartofAccountFinderModalForCustomer', { static: true }) chartofAccountFinderModalforcustomer: ChartofAccountFinderComponent;
    @ViewChild('ChartofAccountFinderModalForVendor', { static: true }) chartofAccountFinderModalForVendor: ChartofAccountFinderComponent;
    @ViewChild('createOrEditModal', { static: true }) modal: ModalDirective;
    @ViewChild('opt4LookupTableModal', { static: true }) opt4LookupTableModal: Opt4LookupTableModalComponent;
    @ViewChild('opt5LookupTableModal', { static: true }) opt5LookupTableModal: Opt5LookupTableModalComponent;
    // @ViewChild('ChangeItemPictureModal', { static: true }) changeItemPictureModal: IcItemPrictureModalComponent;
    //@ViewChild('unitofMeasureModal',{ static: true}) unitofMeasureModal: ICUOMLookupTableModalComponent;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    uploadUrl: string;
    uploadedFiles: any[] = [];

    active = false;
    saving = false;
    flag = false;
    itemPicture: string;
    defTaxAuthDesc: string;
    defCustACDesc: string;
    defVendorACDesc: string;
    TaxClasses: ComboboxItemDto[] = [];
    customerSubLedgers: AccountSubLedgerChartofControlLookupTableDto[] = [];
    vendorSubLedger: AccountSubLedgerChartofControlLookupTableDto[] = [];

    filterText: '';
    seg1IdFilter: '';
    seg1NameFilter: '';
    segment2Filter = '';
    segment3Filter = '';
    maxAllowNegativeFilter: number;
    maxAllowNegativeFilterEmpty: number;
    minAllowNegativeFilter: number;
    minAllowNegativeFilterEmpty: number;
    maxErrSrNoFilter: number;
    maxErrSrNoFilterEmpty: number;
    minErrSrNoFilter: number;
    minErrSrNoFilterEmpty: number;
    maxCostingMethodFilter: number;
    maxCostingMethodFilterEmpty: number;
    minCostingMethodFilter: number;
    minCostingMethodFilterEmpty: number;
    prBookIDFilter = '';
    rtBookIDFilter = '';
    cnsBookIDFilter = '';
    slBookIDFilter = '';
    srBookIDFilter = '';
    trBookIDFilter = '';
    prdBookIDFilter = '';
    pyRecBookIDFilter = '';
    adjBookIDFilter = '';
    asmBookIDFilter = '';
    wsBookIDFilter = '';
    dsBookIDFilter = '';
    maxCurrentLocIDFilter: number;
    maxCurrentLocIDFilterEmpty: number;
    minCurrentLocIDFilter: number;
    minCurrentLocIDFilterEmpty: number;
    opt4Filter = '';
    opt5Filter = '';
    createdByFilter = '';
    seg1Name: string;
    seg2Name: string;
    opt4Name: string;
    opt5Name: string;
    icItem: CreateOrEditICItemDto = new CreateOrEditICItemDto();
    seg3Name: string;
    seg1Setup: string;
    seg2Setup: string;
    seg3Setup: string;

    defSelectedUnit: string = "0"

    unitOfmeasure: ComboboxItemDto[] = [];

    displayDialog: boolean;

    unitMeasure: IC_UNITDto = new IC_UNITDto();

    selectedUnit: IC_UNITDto;

    newUnit: boolean;

    records: IC_UNITDto[];

    isActive: boolean;


    creationDate: Date;
    audtDate: Date;
    manufectureDate: Date;
    expirydate: Date;
    defTaxClassID: number;

    tempTexClasses: any[];
    priceListDetail: any[];



    constructor(
        injector: Injector,
        private _IcItemServiceProxy: IcItemServiceProxy,
        private _icSetupsService: ICSetupsService,
        private _accountSubledgerProxy: AccountSubLedgersServiceProxy,
        private _profileService: ProfileServiceProxy,
        private _unitofMeasure: ICUOMsService,
        private _tokenService: TokenService,
        private _priceListService: PriceListService
    ) {
        super(injector);
    }

    show(update: boolean = false, Segment3ID?: number): void {
        debugger;

        this.initializeModal();

        this.getUnitofMeasure();

        this.creationDate = null;
        this.audtDate = null;
        this.manufectureDate = null;
        this.expirydate = null;
        this.opt4Name = '';
        this.opt5Name = '';
        this.TaxClasses = [];
        if (!update) {
            this.icItem = new CreateOrEditICItemDto();
            this.icItem.itemType = 1;
            this.icItem.itemStatus = 0;
            this.flag = update;
            this.defTaxAuthDesc = "";
            this.defCustACDesc = "";
            this.defVendorACDesc = "";
            this.TaxClasses = [];
            this.seg3Name = "";
            this.seg2Name = ""
            this.seg1Name = "";
            this.records = [];
            this.customerSubLedgers = [];
            this.vendorSubLedger = [];
            // this.getUnitofMeasure();
            this.active = true;
            this.modal.show();
        } else {
            debugger;

            this._IcItemServiceProxy.GetIcItemForEdit(Segment3ID).subscribe(result => {
                this.icItem = result.icItem;

                if (result.icItem.defCustAC) {
                    this.getsubCustomerAccount(result.icItem.defCustAC);
                }

                if (result.icItem.defVendorAC) {
                    this.getsubVendorAccount(result.icItem.defVendorAC);
                }

                this.seg1Name = result.seg1Name;
                this.seg2Name = result.seg2Name;
                this.seg3Name = result.seg3Name;
                this.opt4Name = result.option4Desc;
                this.opt5Name = result.option5Desc;
                this.defVendorACDesc = result.defVendorAccDesc;
                this.defCustACDesc = result.defCustomerAccDesc;
                this.defTaxAuthDesc = result.DefTaxAuthDesc;

                if (this.icItem.creationDate) {
                    this.creationDate = this.icItem.creationDate.toDate();
                }
                if (this.icItem.audtDate) {
                    this.audtDate = this.icItem.audtDate.toDate();
                }
                if (this.icItem.manufectureDate) {
                    this.manufectureDate = this.icItem.manufectureDate.toDate();
                }
                if (this.icItem.expirydate) {
                    this.expirydate = this.icItem.expirydate.toDate();
                }

                this.getitemPicture(result.icItem.itemId);

                this.records = result.icItem.iC_Units;

                this.flag = update;
                this.active = true;
                this.modal.show();
            });
        }
        this.init();
    }


    getDefualtPriceList(){
        debugger;
        this._priceListService.activeListOfPriceList(
            this.filterText,
            '',
            0,
            99999
        ).subscribe(result => {
         debugger;
            this.priceListDetail = result["result"]["items"]

            console.log(result);


        });
    }

    init(): void {
        this.getDefualtPriceList();
        //this.getUnitofMeasure();
        this.unitMeasure.unit = "0"
        this._icSetupsService.getAll(
            this.filterText,
            this.seg1IdFilter,
            this.segment2Filter,
            this.segment3Filter,
            this.maxErrSrNoFilter == null ? this.maxErrSrNoFilterEmpty : this.maxErrSrNoFilter,
            this.minErrSrNoFilter == null ? this.minErrSrNoFilterEmpty : this.minErrSrNoFilter,
            this.maxCostingMethodFilter == null ? this.maxCostingMethodFilterEmpty : this.maxCostingMethodFilter,
            this.minCostingMethodFilter == null ? this.minCostingMethodFilterEmpty : this.minCostingMethodFilter,
            this.prBookIDFilter,
            this.rtBookIDFilter,
            this.cnsBookIDFilter,
            this.slBookIDFilter,
            this.srBookIDFilter,
            this.trBookIDFilter,
            this.prdBookIDFilter,
            this.pyRecBookIDFilter,
            this.adjBookIDFilter,
            this.asmBookIDFilter,
            this.wsBookIDFilter,
            this.dsBookIDFilter,
            this.maxCurrentLocIDFilter == null ? this.maxCurrentLocIDFilterEmpty : this.maxCurrentLocIDFilter,
            this.minCurrentLocIDFilter == null ? this.minCurrentLocIDFilterEmpty : this.minCurrentLocIDFilter,
            this.opt4Filter,
            this.opt5Filter,
            this.createdByFilter,
            null,
            null,
            null,
            0,
            2147483646
        ).subscribe(result => {
            ;
            this.primengTableHelper.totalRecordsCount = result.totalCount;
            if (result.totalCount > 0) {
                this.seg1Setup = result.items[0].segment1;
                this.seg2Setup = result.items[0].segment2;
                this.seg3Setup = result.items[0].segment3;
            }
        });


    }

    showDialogToAdd() {
        //  this.getUnitofMeasure();
        this.defSelectedUnit = "0";
        this.newUnit = true;
        this.unitMeasure = new IC_UNITDto();
        this.unitMeasure.active=true;
        this.displayDialog = true;
    }

    saveUnit() {
        ;
        this.unitMeasure.unit = this.defSelectedUnit;
        let units = [];
        if (this.records) {

            units = [...this.records];
        }

        if (this.newUnit) {
            // if (units.length > 0) {



            let exi = false;

            units.forEach(element => {
                if (element.unit == this.unitMeasure.unit) {
                    abp.message.info("Unit Already in Queue", "Exists");
                    exi = true;
                }

            });
            if (!exi) {
                units.forEach(element => {
                    if (this.unitMeasure.active) {
                        element.active = false;
                    }

                });
                units.push(this.unitMeasure)
            }
        }
        else {
            units.forEach(element => {
                if (element.active) {
                    element.active = false;
                }
            });
            units[this.records.indexOf(this.selectedUnit)] = this.unitMeasure;
        }
        this.records = units;
        this.unitMeasure = null;
        this.displayDialog = false;

    }

    deleteUnit() {

        let index = this.records.indexOf(this.selectedUnit);
        this.records = this.records.filter((val, i) => i != index);
        this.unitMeasure = null;
        this.displayDialog = false;
    }

    onRowSelect(event) {
        ;
        this.newUnit = false;
        this.unitMeasure = this.cloneUnit(event.data);
        this.defSelectedUnit = this.unitMeasure.unit;
        this.displayDialog = true;
    }

    cloneUnit(c: IC_UNITDto): IC_UNITDto {
        let unit = new IC_UNITDto;
        for (let prop in c) {
            unit[prop] = c[prop];
        }
        return unit;
    }

    openSelectICSegment1Modal() {
        this.icSegment1FinderModal.id = this.icItem.seg1Id;
        this.icSegment1FinderModal.displayName = this.seg1Name;
        this.icSegment1FinderModal.show();
    }

    openSelectICSegment2Modal() {
        ;
        this.icSegment2FinderModal.id = this.icItem.seg2Id;
        this.icSegment2FinderModal.displayName = this.seg2Name;
        this.icSegment2FinderModal.show(this.icItem.seg1Id);

    }

    openSelectICSegment3Modal() {
        ;
        this.icSegment3FinderModal.id = this.icItem.seg3Id;
        this.icSegment3FinderModal.displayName = this.seg3Name;
        this.icSegment3FinderModal.show(this.icItem.seg2Id);

    }

    openTaxAuthorityModal() {
        ;
        this.taxAuthorityFinderModal.id = this.icItem.defTaxAuth;
        this.taxAuthorityFinderModal.displayName = this.defTaxAuthDesc;
        this.taxAuthorityFinderModal.show();

    }

    openCustomerAccountModal() {
        ;
        this.chartofAccountFinderModalforcustomer.accid = this.icItem.defCustAC;
        this.chartofAccountFinderModalforcustomer.displayName = this.defCustACDesc;
        this.chartofAccountFinderModalforcustomer.show();

    }

    openVendorAccountModal() {
        this.chartofAccountFinderModalForVendor.accid = this.icItem.defVendorAC;
        this.chartofAccountFinderModalForVendor.displayName = this.defVendorACDesc;
        this.chartofAccountFinderModalForVendor.show();

    }

    openUnitofMeasureModal() {
        this.chartofAccountFinderModalForVendor.accid = this.icItem.defVendorAC;
        this.chartofAccountFinderModalForVendor.displayName = this.defVendorACDesc;
        this.chartofAccountFinderModalForVendor.show();

    }

    setICSegment1IdNull() {
        this.icItem.seg1Id = '';
        this.seg1Name = '';
        this.icItem.seg2Id = '';
        this.seg2Name = '';
        this.icItem.seg3Id = null;

    }

    setICSegment2IdNull() {
        ;
        this.icItem.seg2Id = '';
        this.seg2Name = '';
        this.icItem.seg3Id = null;
        this.seg3Name = '';
    }

    setICSegment3IdNull() {
        ;
        this.icItem.seg3Id = null;
        this.seg3Name = '';

        this.icItem.itemId = null;
    }

    setTaxAuthorityNull() {
        this.icItem.defTaxAuth = null;
        this.defTaxAuthDesc = '';
        this.TaxClasses = [];
    }

    setCustomerAccountNull() {
        this.icItem.defCustAC = null;
        this.defCustACDesc = '';
        this.customerSubLedgers = [];
    }

    setVendorAccountNull() {
        this.icItem.defVendorAC = null;
        this.defVendorACDesc = '';
        this.vendorSubLedger = [];
    }

    setUnitofMeasureModal() {

    }

    getNewICSegment1Id() {
        ;
        this.icItem.seg1Id = this.icSegment1FinderModal.id;
        this.seg1Name = this.icSegment1FinderModal.displayName;
        this.icItem.seg2Id = null;
        this.seg2Name = '';
        this.icItem.seg3Id = null;

    }

    getNewICSegment2Id() {
        this.icItem.seg2Id = this.icSegment2FinderModal.id;
        this.seg2Name = this.icSegment2FinderModal.displayName;
    }

    getNewICSegment3Id() {
        this.icItem.seg3Id = this.icSegment3FinderModal.id;
        this.seg3Name = this.icSegment3FinderModal.displayName;
        ;
        if (this.icItem.seg3Id != '' && this.icItem.seg3Id != null) {

            this._IcItemServiceProxy.GetIcItemMaxId(this.icItem.seg3Id).subscribe((result) => {
                ;
                this.icItem.itemId = result.toString();
            });
        } else {
            this.icItem.itemId = null;
        }


    }


    getNewTaxAuthority() {
        this.icItem.defTaxAuth = this.taxAuthorityFinderModal.id;
        this.defTaxAuthDesc = this.taxAuthorityFinderModal.displayName;
        if (this.icItem.defTaxAuth) {

            this.getTaxClasses(this.icItem.defTaxAuth);
        }
        else {
            this.TaxClasses = [];
        }
    }

    getCustomerAccount() {
        this.icItem.defCustAC = this.chartofAccountFinderModalforcustomer.accid;
        this.defCustACDesc = this.chartofAccountFinderModalforcustomer.displayName;
        if (this.icItem.defCustAC) {

            this.getsubCustomerAccount(this.icItem.defCustAC);

        };
    }

    getVendorAccount() {

        this.icItem.defVendorAC = this.chartofAccountFinderModalForVendor.accid;
        this.defVendorACDesc = this.chartofAccountFinderModalForVendor.displayName;
        if (this.chartofAccountFinderModalForVendor.accid) {
            this.getsubVendorAccount(this.icItem.defVendorAC);
        }

    }

    getTaxClasses(taxAuth: string) {
        this._accountSubledgerProxy.getAllTaxClassesForCombobox(taxAuth).subscribe(result => {
            this.TaxClasses = result.items;
        });
    }

    getsubCustomerAccount(account: string) {
        this._accountSubledgerProxy.getAllAccountSubledger_lookup(
            account,
            '',
            0,
            100000
        ).subscribe(result => {

            result.items.forEach(element => {
                element.id = element.id.slice(15)
            });
            this.customerSubLedgers = result.items;
        });
    }

    getsubVendorAccount(account: string) {
        this._accountSubledgerProxy.getAllAccountSubledger_lookup(
            account,
            '',
            0,
            100000
        ).subscribe(result => {
            result.items.forEach(element => {
                element.id = element.id.slice(15)
            });
            this.vendorSubLedger = result.items;
        });
    }

    getUnitofMeasure(): void {
        ;
        this._unitofMeasure.getUnitofMeasureForCombobox(
        ).subscribe(result => {
            ;
            this.unitOfmeasure = result.items
        })
    }

    maxConverFilter: number;
    maxConverFilterEmpty: number;
    minConverFilter: number;
    minConverFilterEmpty: number;
    activeFilter = -1;

    maxCreateDateFilter: moment.Moment;
    minCreateDateFilter: moment.Moment;
    audtUserFilter = '';
    maxAudtDateFilter: moment.Moment;
    minAudtDateFilter: moment.Moment;

    GetConversions(param) {
        ;
        if (this.defSelectedUnit !== "0") {
            this._unitofMeasure.getAll('', param.target.value,
                '',
                this.maxConverFilter == null ? this.maxConverFilterEmpty : this.maxConverFilter,
                this.minConverFilter == null ? this.minConverFilterEmpty : this.minConverFilter,
                this.activeFilter,
                this.createdByFilter,
                this.maxCreateDateFilter,
                this.minCreateDateFilter,
                this.audtUserFilter,
                this.maxAudtDateFilter,
                this.minAudtDateFilter,
                null,
                0,
                5
            )
                .subscribe(result => {

                    console.log(result.items[0].conver);
                    this.unitMeasure.conver = result.items[0].conver;
                })
        }


    }


    save(): void {
        
        this.saving = true;
        this.icItem.flag = this.flag;

        if (this.creationDate) {
            if (!this.icItem.creationDate) {
                this.icItem.creationDate = moment(this.creationDate).startOf('day');
            }
            else {
                this.icItem.creationDate = moment(this.creationDate);
            }
        }
        else {
            this.icItem.creationDate = null;
        }
        if (this.audtDate) {
            if (!this.icItem.audtDate) {
                this.icItem.audtDate = moment(this.audtDate).startOf('day');
            }
            else {
                this.icItem.audtDate = moment(this.audtDate);
            }
        }
        else {
            this.icItem.audtDate = null;
        }
        if (this.manufectureDate) {
            if (!this.icItem.manufectureDate) {
                this.icItem.manufectureDate = moment(this.manufectureDate).startOf('day');
            }
            else {
                this.icItem.manufectureDate = moment(this.manufectureDate);
            }
        }
        else {
            this.icItem.manufectureDate = null;
        }
        if (this.expirydate) {
            if (!this.icItem.expirydate) {
                this.icItem.expirydate = moment(this.expirydate).startOf('day');
            }
            else {
                this.icItem.expirydate = moment(this.expirydate);
            }
        }
        else {
            this.icItem.expirydate = null;
        }

        if (this.icItem.defTaxAuth) {
            this.icItem.defTaxClassID = this.defTaxClassID;
        }
        debugger;
        if(this.records.length>0){
            this.icItem.iC_Units = this.records;
            this.icItem.iC_Units.forEach(element => {
                element.itemId = this.icItem.itemId;
                element.id=null;
                if (element.active) {
                    this.icItem.conver = element.conver;
                    this.icItem.stockUnit = element.unit;
                }
            });
            this._IcItemServiceProxy.createOrEdit(this.icItem)
                .pipe(finalize(() => { this.saving = false; }))
                .subscribe(() => {
                    this.uploader.uploadAll();
    
                    this.message.confirm("Press 'Yes' for create new Item",this.l('SavedSuccessfully'),(isConfirmed) => {
                        if (isConfirmed) {
                            this._IcItemServiceProxy.GetIcItemMaxId(this.icItem.seg3Id).subscribe((result) => {
                                ;
                                this.icItem.itemId = result.toString();
                            });
                            this.opt5Name = '';
                            this.opt4Name = '';
                            this.defVendorACDesc = '';
                            this.defCustACDesc = '';
                            this.defTaxAuthDesc = '';
                            this.customerSubLedgers = [];
                            this.vendorSubLedger = [];
                            this.TaxClasses = [];
                            this.records = [];
                            this.croppedImage = null;
    
    
                            this.flag = false;
                            let seg1Id = this.icItem.seg1Id;
                            let seg2Id = this.icItem.seg2Id;
                            let seg3Id = this.icItem.seg3Id;
                            this.icItem.itemType = 0;
                            this.icItem.itemStatus = 0;
    
                            this.icItem = new CreateOrEditICItemDto();
    
                            this.icItem.seg1Id = seg1Id;
                            this.icItem.seg2Id = seg2Id;
                            this.icItem.seg3Id = seg3Id;
                            this.icItem.itemType = 0;
                            this.icItem.itemStatus = 0;
    debugger;
    
                            this.modalSave.emit(null);
                        }else{
                        this.close();
                            this.modalSave.emit(null);
                        }
                    });
    
                    // this.uploader.uploadAll();
                    // this.notify.info(this.l('SavedSuccessfully'));
                    //  this.close();
                    //  this.modalSave.emit(null);
                });
        }
       else {
           this.message.info("Please Enter Unit Of Measure");
           this.saving = false;
       }


    }

    close(): void {

        this.active = false;
        this.croppedImage = '';
        //this.imageChangedEvent = '';
        //this.uploader.clearQueue();
        this.modal.hide();
    }



    // upload completed event
    // onUpload(event): void {
    //     ;
    //     for (const file of event.files) {
    //         this.uploadedFiles.push(file);
    //     }
    // }

    // myUploader(event) {
    //     ;
    //     //event.files == files to upload
    // }

    onBeforeSend(event): void {
        event.xhr.setRequestHeader('Authorization', 'Bearer ' + abp.auth.getToken());
    }

    openOption4() {
        this.opt4LookupTableModal.show();
    }

    openOption5() {
        this.opt5LookupTableModal.show();
    }

    getOpt4() {
        this.icItem.opt4 = this.opt4LookupTableModal.data.optID;
        this.opt4Name = this.opt4LookupTableModal.data.descp;
    }

    getOpt5() {
        this.icItem.opt5 = this.opt5LookupTableModal.data.optID;
        this.opt5Name = this.opt5LookupTableModal.data.descp;
    }

    setOpt5Null() {
        this.icItem.opt5 = undefined;
        this.opt5Name = undefined;
    }

    setOpt4Null() {
        this.icItem.opt4 = undefined;
        this.opt4Name = undefined;
    }


    public uploader: FileUploader;
    public temporaryPictureUrl: string;


    private maxProfilPictureBytesUserFriendlyValue = 5;
    private temporaryPictureFileName: string;
    private _uploaderOptions: FileUploaderOptions = {};
    containWithinAspectRatio = true;
    imageChangedEvent: any = '';
    showCropper = false;


    croppedImage: any = '';


    fileChangeEvent(event: any): void {
        ;
        if (event.target.files[0].size > 5242880) { //5MB
            this.message.warn(this.l('ProfilePicture_Warn_SizeLimit', this.maxProfilPictureBytesUserFriendlyValue));
            return;
        }
        this.imageChangedEvent = event;
    }

    imageCropped(event: ImageCroppedEvent) {
        this.croppedImage = event.base64;
    }

    imageCroppedFile(file: File) {


        let files: File[] = [file];
        this.uploader.clearQueue();
        this.uploader.addToQueue(files);
    }
   
    imageLoaded() {
        this.showCropper = true;
        console.log('Image loaded');
    }

    cropperReady(sourceImageDimensions: Dimensions) {
        console.log('Cropper ready', sourceImageDimensions);
    }

    loadImageFailed() {
        console.log('Load failed');
    }

    ClearIamge() {
        this.imageChangedEvent = '';
        this.croppedImage = '';
    }


    initializeModal(): void {
        ;
        // this.active = true;
        this.temporaryPictureUrl = '';
        this.temporaryPictureFileName = '';
        this.initFileUploader();
    }

    initFileUploader(): void {
        debugger
        this.uploader = new FileUploader(
            {
                url: AppConsts.remoteServiceBaseUrl + '/IcItem/UploadItemPicture'
            }
        );

        this._uploaderOptions.autoUpload = false;
        this._uploaderOptions.authToken = 'Bearer ' + this._tokenService.getToken();
        this._uploaderOptions.removeAfterUpload = true;
        this.uploader.onAfterAddingFile = (file) => {
            file.withCredentials = false;
        };

        this.uploader.onBuildItemForm = (fileItem: FileItem, form: any) => {
debugger;
            form.append('FileType', fileItem.file.type);
            form.append('FileName', 'ItemPicture');
            form.append('FileToken', this.guid());
        };

        this.uploader.onSuccessItem = (item, response, status) => {

            const resp = <IAjaxResponse>JSON.parse(response);
            if (resp.success) {
                debugger;

                this.updateItemPicture(resp.result.fileToken);
            } else {
                this.message.error(resp.error.message);
            }
        };

        this.uploader.setOptions(this._uploaderOptions);
    }

    updateItemPicture(fileToken: string): void {
        const input = new UpdateItemPictureInput();
        debugger
            input.fileToken = fileToken;


        input.x = 0;
        input.y = 0;
        input.width = 0;
        input.height = 0;
        if(!this.flag)
            input.itemID = this.icItem.seg3Id + '-' + this.icItem.itemId;
        else
            input.itemID = this.icItem.itemId;



        this.saving = true;
        this._IcItemServiceProxy.updateItemPicture(input)
            .pipe(finalize(() => { this.saving = false; }))
            .subscribe(() => {
                abp.event.trigger('profilePictureChanged');
//                 this.message.confirm("Press 'Yes' for create new Item",this.l('SavedSuccessfully'),(isConfirmed) => {
//                     if (isConfirmed) {
//                         this._IcItemServiceProxy.GetIcItemMaxId(this.icItem.seg3Id).subscribe((result) => {
//                             ;
//                             this.icItem.itemId = result.toString();
//                         });
//                         this.opt5Name = '';
//                         this.opt4Name = '';
//                         this.defVendorACDesc = '';
//                         this.defCustACDesc = '';
//                         this.defTaxAuthDesc = '';
//                         this.customerSubLedgers = [];
//                         this.vendorSubLedger = [];
//                         this.TaxClasses = [];
//                         this.records = [];
//                         this.croppedImage = null;


//                         this.flag = false;
//                         let seg1Id = this.icItem.seg1Id;
//                         let seg2Id = this.icItem.seg2Id;
//                         let seg3Id = this.icItem.seg3Id;

//                         this.icItem = new CreateOrEditICItemDto();

//                         this.icItem.seg1Id = seg1Id;
//                         this.icItem.seg2Id = seg2Id;
//                         this.icItem.seg3Id = seg3Id;
// debugger;

//                         this.modalSave.emit(null);
//                     }else{
//                         this.close();
//                         this.modalSave.emit(null);
//                     }
//                 });

            });
    }

    guid(): string {
        function s4() {
            return Math.floor((1 + Math.random()) * 0x10000)
                .toString(16)
                .substring(1);
        }
        return s4() + s4() + '-' + s4() + '-' + s4() + '-' + s4() + '-' + s4() + s4() + s4();
    }

    getitemPicture(itemID: string): void {
        this._IcItemServiceProxy.getItemPicture(itemID).subscribe(result => {
            if (result && result.itemPicture) {
                this.itemPicture = 'data:image/jpeg;base64,' + result.itemPicture;
                this.croppedImage = this.itemPicture;
                this.imageChangedEvent = this.itemPicture;
            }
        });
    }
}
