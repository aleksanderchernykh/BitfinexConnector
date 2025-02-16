import { Component } from '@angular/core';

interface CurrencyBalance {
  currency: string;
  amount: number;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent {
  title = 'BitfinexConnectorAppTest';

  сurrency: string[] = [
    "BTC",
    "XRP",
    "USDT",
    "XMR",
    "DASH"
  ]

  wallet: CurrencyBalance[] = [
    { currency: "BTC", amount: 1 },
    { currency: "XRP", amount: 15000 },
    { currency: "XMR", amount: 50 },
    { currency: "DASH", amount: 5 },
  ];
}

function createCurrencyPairs(currency: string[], wallet: CurrencyBalance[]): [string, string][] {
  const pairs: [string, string][] = [];

  for (const walletItem of wallet) {
      for (const curr of currency) {
          if (walletItem.currency !== curr) {
              pairs.push([walletItem.currency, curr]);
          }
      }
  }

  return pairs;
}
