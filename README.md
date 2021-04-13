# BinanceWalletSummary
Binance Wallet Summary


  public BinanceCoreService(IBinanceClient client)
        {
            _binanceClient = client;
            _binanceClient.SetApiCredentials("apiKey", "apiSecret");    ///Burayı replace edin
        }
