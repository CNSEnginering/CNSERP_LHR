import {
    Component,
    ViewChild,
    Injector,
    Output,
    EventEmitter
} from "@angular/core";
import { ModalDirective } from "ngx-bootstrap";
import { AppComponentBase } from "@shared/common/app-component-base";
import { InventoryGlLinkDto } from "../shared/dto/inventory-glLink-dto";
import { ItemPricingDto } from "../shared/dto/itemPricing-dto";

@Component({
    selector: "viewItemPricingModal",
    templateUrl: "./view-itemPricing-modal.component.html"
})
export class ViewItemPricingComponent extends AppComponentBase {
    @ViewChild("viewItemPricingModal", { static: true }) modal: ModalDirective;
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();

    active = false;
    saving = false;

    item: ItemPricingDto;

    constructor(injector: Injector) {
        super(injector);
        this.item = new ItemPricingDto();
    }

    show(item: ItemPricingDto): void {
        this.item = item["itemPricing"];
        this.active = true;
        this.modal.show();
    }

    close(): void {
        this.active = false;
        this.modal.hide();
    }
}
