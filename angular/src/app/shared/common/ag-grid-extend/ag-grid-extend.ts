export class AgGridExtend {
    // this puts commas into the number eg 1000 goes to 1,000
    formatNumber(params) {
        //return Math.floor(params.value).toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");

        return Math.abs(params.value)
            .toString()
            .replace(/(\d)(?=(\d{3})+(?!\d))/g, "$1,");
    }

    // this puts + icon into the cell with green background,
    addIconCellRendererFunc(params) {
        return '<i class="fa fa-plus-circle fa-lg" style="color: green;margin-left: -9px;cursor: pointer;" ></i>';
    }
}
