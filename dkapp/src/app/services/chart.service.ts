import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ChartService {
  constructor() {}

  getPrimary(): string {
    const documentStyle = getComputedStyle(document.documentElement);
    return documentStyle.getPropertyValue('--pi-primary-color');
  }

  getBasicOptions(
    textColor: string,
    surfaceBorder: string,
    textColorSecondary: string
  ): ChartBasicOptions {
    return {
      plugins: {
        legend: {
          labels: {
            color: textColor,
          },
        },
      },
      legend: {
        labels: {
          fontColor: textColor,
        },
      },
      scales: {
        x: {
          grid: {
            color: surfaceBorder,
          },
          ticks: {
            color: textColorSecondary,
          },
        },
        y: {
          grid: {
            color: surfaceBorder,
          },
          ticks: {
            color: textColorSecondary,
          },
        },
      },
    } as ChartBasicOptions;
  }

  getDefaultBasicOptions(): ChartBasicOptions {
    const documentStyle = getComputedStyle(document.documentElement);
    const textColor = documentStyle.getPropertyValue('--pi-text-color');
    const textColorSecondary = documentStyle.getPropertyValue('--pi-text-muted-color');
    const surfaceBorder = documentStyle.getPropertyValue('--pi-content-border-color');

    const options = this.getBasicOptions(textColor, surfaceBorder, textColorSecondary);
    return options;
  }

  getDefaultBasicData(): ChartBasicData {
    return { labels: ['A', 'B', 'C'], datasets: [{ label: 'Unknown' }] } as ChartBasicData;
  }
}

export interface ChartBasicOptions {
  plugins: {
    legend: {
      labels: {
        color: string;
      };
    };
  };
  legend: {
    labels: {
      fontColor: string;
    };
  };
  scales: {
    x: {
      grid: {
        color: string;
      };
      ticks: {
        color: string;
      };
    };
    y: {
      grid: {
        color: string;
      };
      ticks: {
        color: string;
      };
    };
  };
}

export interface ChartDataSet {
  label: string;
  data: number[];
  fill: boolean;
  borderColor: string;
  tension: number;
}

export interface ChartBasicData {
  labels: string[];
  datasets: ChartDataSet[];
}
