import { Component, OnInit } from '@angular/core';
import { DropDownData } from '../../common/dropdown/dropdown.component';

@Component({
    selector: 'report',
    templateUrl: './report.component.html'
})
export class ReportComponent implements OnInit {
    private listData: DropDownData[] = [];
    private typeSelected: number = ReportType.DebtBook;
    private ReportType: any = ReportType;

    public ngOnInit(): void {
        this.listData.push(new DropDownData(ReportType.DebtBook, 'Bạn đọc nợ sách'));
        this.listData.push(new DropDownData(ReportType.TopBook, 'Sách được mượn nhiều'));
        this.listData.push(new DropDownData(ReportType.ReadStatistics, 'Lượt mượn/ trả sách'));
        this.listData.push(new DropDownData(ReportType.BorrowStatus, 'Tình trạng mượn sách'));
        this.listData.push(new DropDownData(ReportType.CategoryReport, 'Thống kê đầu sách'));
    }

    private selectType(type: DropDownData): void {
        this.typeSelected = type.key;
    }
}

enum ReportType {
    DebtBook = 1,
    TopBook = 2,
    ReadStatistics = 3,
    BorrowStatus = 4,
    CategoryReport = 5
}
