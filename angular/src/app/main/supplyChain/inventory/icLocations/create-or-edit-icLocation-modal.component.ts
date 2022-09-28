import {
	Component,
	ViewChild,
	Injector,
	Output,
	EventEmitter
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { finalize } from "rxjs/operators";
import { AppComponentBase } from "@shared/common/app-component-base";
import * as moment from "moment";
import { ICLocationDto } from "../shared/dto/ic-locations-dto";
import { ICLocationsService } from "../shared/services/ic-locations.service";
import { GetDataService } from "../shared/services/get-data.service";

@Component({
	selector: "createOrEditICLocationModal",
	templateUrl: "./create-or-edit-icLocation-modal.component.html"
})
export class CreateOrEditICLocationModalComponent extends AppComponentBase {
	@ViewChild("createOrEditModal", { static: true }) modal: ModalDirective;

	@Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

	active = false;
	saving = false;
	locations: any;
	eLoc1List;
	eLoc2List;
	eLoc3List;
	eLoc4List;
	eLoc5List;

	icLocation: ICLocationDto = new ICLocationDto();

	createDate: Date;
	audtDate: Date;

	constructor(
		injector: Injector,
		private _icLocationsService: ICLocationsService,
		private _getDataService: GetDataService
	) {
		super(injector);
	}

	getLocations(target: string): void {
		debugger;
		this._getDataService.getList(target).subscribe(result => {
			this.locations = result;
		});
	}

	getRegions(id: any) {
		this._icLocationsService.getRegions(id).subscribe(res => {
			this.eLoc1List = res["result"];
		});
	}

	show(icLocationId?: number): void {
		this.createDate = null;
		this.audtDate = null;

		this.eLoc1List = null;
		this.eLoc2List = null;
		this.eLoc3List = null;
		this.eLoc4List = null;
		this.eLoc5List = null;

		this.getLocations("PICLocations");
		this.getRegions(0);

		if (!icLocationId) {
			debugger;
			this.icLocation = new ICLocationDto();
			this.icLocation.id = icLocationId;
			this.icLocation.parentID = 0;

			this._icLocationsService.getMaxLocId().subscribe(result => {
				this.icLocation.locID = result;
			});

			this.active = true;
			this.modal.show();
		} else {
			this._icLocationsService
				.getICLocationForEdit(icLocationId)
				.subscribe(result => {
					this.icLocation = result;

					if (this.icLocation.eLoc1 > 0) {
						this.changeEloc("eLoc1");
					}

					if (this.icLocation.eLoc2 > 0) {
						this.changeEloc("eLoc2");
					}

					if (this.icLocation.eLoc3 > 0) {
						this.changeEloc("eLoc3");
					}

					if (this.icLocation.eLoc4 > 0) {
						this.changeEloc("eLoc4");
					}

					if (this.icLocation.createDate) {
						this.createDate = this.icLocation.createDate.toDate();
					}
					if (this.icLocation.audtDate) {
						this.audtDate = this.icLocation.audtDate.toDate();
					}

					this.active = true;
					this.modal.show();
				});
		}
	}

	changeEloc(type: string): void {
		debugger;
		switch (type) {
			case "eLoc1":
				if (this.icLocation.eLoc1 > 0) {
					this._icLocationsService
						.getRegions(this.icLocation.eLoc1)
						.subscribe(res => {
							this.eLoc2List = res["result"];
						});
				}
				break;
			case "eLoc2":
				if (this.icLocation.eLoc2 > 0) {
					this._icLocationsService
						.getRegions(this.icLocation.eLoc2)
						.subscribe(res => {
							this.eLoc3List = res["result"];
						});
				}
				break;
			case "eLoc3":
				if (this.icLocation.eLoc3 > 0) {
					this._icLocationsService
						.getRegions(this.icLocation.eLoc3)
						.subscribe(res => {
							this.eLoc4List = res["result"];
						});
				}
				break;
			case "eLoc4":
				if (this.icLocation.eLoc4 > 0) {
					this._icLocationsService
						.getRegions(this.icLocation.eLoc4)
						.subscribe(res => {
							this.eLoc5List = res["result"];
						});
				}
				break;
		}
	}

	save(): void {
		this.saving = true;

		debugger;

		if (this.createDate) {
			if (!this.icLocation.createDate) {
				this.icLocation.createDate = moment(this.createDate).startOf(
					"day"
				);
			} else {
				this.icLocation.createDate = moment(this.createDate);
			}
		} else {
			this.icLocation.createDate = null;
		}
		if (this.audtDate) {
			if (!this.icLocation.audtDate) {
				this.icLocation.audtDate = moment(this.audtDate).startOf("day");
			} else {
				this.icLocation.audtDate = moment(this.audtDate);
			}
		} else {
			this.icLocation.audtDate = null;
		}

		this.icLocation.audtDate = moment();
		this.icLocation.audtUser = this.appSession.user.userName;

		if (!this.icLocation.id) {
			this.icLocation.createDate = moment();
			this.icLocation.createdBy = this.appSession.user.userName;
		}

		this._icLocationsService
			.createOrEdit(this.icLocation)
			.pipe(
				finalize(() => {
					this.saving = false;
				})
			)
			.subscribe(result => {
				if (result == "OK") {
					this.notify.info(this.l("SavedSuccessfully"));
					this.message.confirm("Press 'Yes' for New Location", this.l('SavedSuccessfully'), ((isConfirmed) => {
						if (isConfirmed) {
							let seg1ID = this.icLocation.locID;
							this.icLocation = new ICLocationDto();

							this.show();
							// this.flag = false;
							this.modalSave.emit(null);
						}
						else {
							this.notify.info(this.l('SavedSuccessfully'));
							this.close();
							this.modalSave.emit(null);
						}
					}));
				} else if (result == "Present") {
					this.notify.error(this.l("LocationIdPresent"));
				} else {
					this.notify.error(this.l("InputError"));
				}
			});
	}

	close(): void {
		this.active = false;
		this.modal.hide();
	}
}
