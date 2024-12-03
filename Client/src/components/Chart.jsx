import { useLoaderData } from "react-router-dom";
import {
  BarChart,
  Bar,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  Legend,
  ResponsiveContainer,
} from "recharts";

import colors from "tailwindcss/colors";

import { sumTransactionsByPeriod } from "../utils/calc";

const Chart = ({ periodType }) => {
  const transactions = useLoaderData();

  const data = sumTransactionsByPeriod(transactions, periodType);

  return (
    <div>
      <ResponsiveContainer width="100%" height={200}>
        <BarChart data={data}>
          <CartesianGrid strokeDasharray="3 3" />
          <XAxis dataKey="name" />
          <YAxis />
          <Tooltip />
          <Legend />
          <Bar dataKey="income" fill={colors.green[500]} />
          <Bar dataKey="expenses" fill={colors.red[400]} />
        </BarChart>
      </ResponsiveContainer>
    </div>
  );
};

export default Chart;
