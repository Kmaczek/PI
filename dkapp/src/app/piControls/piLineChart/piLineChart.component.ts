import { Component, Input, OnInit } from '@angular/core';
import * as $ from 'jquery';
declare var Chart: any;

@Component({
    selector: 'piLineChart',
    templateUrl: './piLineChart.component.html',
    styleUrls: ['./piLineChart.component.css']
})
export class PiLineChartComponent implements OnInit
{
    private ctx: HTMLElement;
    private chart: any;

    constructor()
    {

    }

    @Input() public labels = [];
    @Input() public data = [];

    ngOnInit()
    {
        this.ctx = document.getElementById('myChart');
        //this.loadChart();
    }

    public loadChart()
    {
        this.chart = new Chart(this.ctx, {
            type: 'line',
            data: {
                labels: this.labels,//['2021-02-12', '2021-02-13','2021-02-14','2021-02-15','2021-02-16', '2021-02-17'],
                datasets: [{
                    label: 'AvgPrice',
                    data: this.data,//[6300, 6400, 6450, 6500, 6500, 6520],
                    borderWidth: 1
                }]
            },
            options: {
                scales: {
                    yAxes: [{
                        ticks: {
                            beginAtZero: false
                        }
                    }],
                    // xAxes: [{
                    //     type: 'category',
                    //     labels: ['January', 'February', 'March', 'April', 'May', 'June']
                    // }]
                }
            }
        });

        // var myChart = new Chart(this.ctx, {
        //     //type: 'bar',
        //     data: {
        //         labels: ['2021-02-12', '2021-02-13','2021-02-14','2021-02-15','2021-02-16',],
        //         datasets: [{
        //             label: 'AvgPrice',
        //             data: [6300, 6400, 6450, 6500, 6500, 6520],
        //             borderWidth: 1
        //         }]
        //     }
        // options: {
        //     scales: {
        //         yAxes: [{
        //             ticks: {
        //                 beginAtZero: true
        //             }
        //         }]
        //     }
        // }
        // });
    }

}
