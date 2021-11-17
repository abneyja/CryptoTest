# CryptoTest

Cryto currency test application that returns usd based prices for cryto currency from various exchanges.

It also provides a moving average for a range of trades completed within a user defined time period. 
The following information is saved to a .csv file. Each label is a column in the file.

starting_date,	interval_timespan,	average_price,	trade_count,	moving_average,	interval_size

The user can configure the interval number for calculating the moving average, default is 3.

The user can also configure the time duration of each interval in minutes, default is 1 minute.

The configuration file is called 
