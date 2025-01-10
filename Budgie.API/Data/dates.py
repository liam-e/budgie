import os
import sys
import pandas as pd
from datetime import date

os.chdir(os.path.dirname(os.path.abspath(sys.argv[0])))

df = pd.read_csv("SeedData/TransactionsOriginal.csv", parse_dates=True)

today = pd.Timestamp(date.today())

df.Date = pd.to_datetime(df.Date)

min_period = df.Date.apply(lambda d : today - d).min()

df.Date = df.Date.apply(lambda d : d + min_period)

df.to_csv("SeedData/Transactions.csv", index=False)