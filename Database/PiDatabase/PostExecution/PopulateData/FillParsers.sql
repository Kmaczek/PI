
IF NOT EXISTS (SELECT * FROM price.ParserType WHERE [Name] = 'XcomPriceParser')
INSERT INTO price.ParserType ([Name]) VALUES ('XcomPriceParser')

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.x-kom.pl/p/507702-karta-graficzna-nvidia-asus-geforce-rtx-2060-dual-evo-oc-6gb-gddr6.html')
INSERT price.Parser VALUES ( 'https://www.x-kom.pl/p/507702-karta-graficzna-nvidia-asus-geforce-rtx-2060-dual-evo-oc-6gb-gddr6.html', 1, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.x-kom.pl/p/500091-procesor-amd-ryzen-9-amd-ryzen-9-3900x.html')
INSERT price.Parser VALUES ( 'https://www.x-kom.pl/p/500091-procesor-amd-ryzen-9-amd-ryzen-9-3900x.html', 1, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.x-kom.pl/p/630588-pamiec-ram-ddr4-crucial-32gb-2x16gb-3600mhz-cl16-ballistix-black.html')
INSERT price.Parser VALUES ( 'https://www.x-kom.pl/p/630588-pamiec-ram-ddr4-crucial-32gb-2x16gb-3600mhz-cl16-ballistix-black.html', 1, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,798/n,kupiec-platki-owsiane-gorskie/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,798/n,kupiec-platki-owsiane-gorskie/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,104093/n,graal-filety-z-makreli-w-oleju/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,104093/n,graal-filety-z-makreli-w-oleju/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,33651/n,dr.-oetker-guseppe-pizza-z-salami/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,33651/n,dr.-oetker-guseppe-pizza-z-salami/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,1276/n,piatnica-smietana-18/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,1276/n,piatnica-smietana-18/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,14760/n,president-twarog-sernikowy-(wiaderko)/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,14760/n,president-twarog-sernikowy-(wiaderko)/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,108907/n,foxy-tornado-papierowy-recznik-3-warstwowy/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,108907/n,foxy-tornado-papierowy-recznik-3-warstwowy/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,3380/n,frisco-fresh-schab-wieprzowy-bez-kosci/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,3380/n,frisco-fresh-schab-wieprzowy-bez-kosci/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,1589/n,frisco-fish-losos-norweski-filet-ze-skora---swiezy-(200g-300g)/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,1589/n,frisco-fish-losos-norweski-filet-ze-skora---swiezy-(200g-300g)/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,883/n,cirio-passata-rustica-(przecier-pomidorowy)/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,883/n,cirio-passata-rustica-(przecier-pomidorowy)/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,2198/n,lipton-earl-grey-herbata-czarna-classic-50-torebek/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,2198/n,lipton-earl-grey-herbata-czarna-classic-50-torebek/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,8883/n,frisco-fresh-mieso-mielone-wieprzowo-wolowe/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,8883/n,frisco-fresh-mieso-mielone-wieprzowo-wolowe/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,40546/n,serenada-ser-gouda-w-kawalku/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,40546/n,serenada-ser-gouda-w-kawalku/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,5245/n,hochland-ser-zolty-w-plastrach---gouda/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,5245/n,hochland-ser-zolty-w-plastrach---gouda/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,99525/n,melvit-maka-zytnia-(do-wypieku-domowego-chleba)/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,99525/n,melvit-maka-zytnia-(do-wypieku-domowego-chleba)/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,3265/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,3265/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,4030/n,frisco-fresh-marchew-luz/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,4030/n,frisco-fresh-marchew-luz/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,3752/n,frisco-fish-makrela-tusza-wedzona-na-goraco-(200g-300g)/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,3752/n,frisco-fish-makrela-tusza-wedzona-na-goraco-(200g-300g)/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,13500/n,mlekovita-maslo-polskie-extra/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,13500/n,mlekovita-maslo-polskie-extra/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,5685/n,lisner-pastella-pasta-kanapkowa-z-suszonymi-pomidorami/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,5685/n,lisner-pastella-pasta-kanapkowa-z-suszonymi-pomidorami/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,106969/n,piatnica-skyr-jogurt-typu-islandzkiego-waniliowy/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,106969/n,piatnica-skyr-jogurt-typu-islandzkiego-waniliowy/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,4094/n,frisco-fresh-banany-premium-kisc-4-6-szt./stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,4094/n,frisco-fresh-banany-premium-kisc-4-6-szt./stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,2999/n,frisco-fresh-szczypiorek---peczek/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,2999/n,frisco-fresh-szczypiorek---peczek/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,91129/n,sokolow-excellent-line-boczek-surowy-wedzony---plastry/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,91129/n,sokolow-excellent-line-boczek-surowy-wedzony---plastry/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,100771/n,wybiegane-kury-jaja-kurze-z-wolnego-wybiegu-rozmiar-m-10-szt/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,100771/n,wybiegane-kury-jaja-kurze-z-wolnego-wybiegu-rozmiar-m-10-szt/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,3409/n,frisco-fresh-cebula-krajowa-4-6-szt./stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,3409/n,frisco-fresh-cebula-krajowa-4-6-szt./stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,18661/n,hochland-kanapkowy-ser-twarogowy-ze-szczypiorkiem/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,18661/n,hochland-kanapkowy-ser-twarogowy-ze-szczypiorkiem/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,7769/n,san-holenderskie-polslodkie-herbatniki-w-mlecznej-czekoladzie/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,7769/n,san-holenderskie-polslodkie-herbatniki-w-mlecznej-czekoladzie/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,10238/n,frisco-fresh-filet-z-kurczaka/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,10238/n,frisco-fresh-filet-z-kurczaka/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

IF NOT EXISTS (SELECT * FROM price.Parser WHERE [Uri] = 'https://www.frisco.pl/pid,3429/n,frisco-fresh-pomidory-krajowe-3-4-szt.-tacka/stn,product')
INSERT price.Parser VALUES ( 'https://www.frisco.pl/pid,3429/n,frisco-fresh-pomidory-krajowe-3-4-szt.-tacka/stn,product', 2, NULL, 0, GETDATE(), GETDATE(), '2021-01-01', '9999-01-01' )

