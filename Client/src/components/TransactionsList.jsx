import Transaction from "../components/Transaction";

const TransactionsList = ({ transactions }) => {
  return (
    <div className="space-y-5">
      <div>
        {transactions &&
          transactions.map((t, idx) => (
            <Transaction key={idx} idx={idx} data={t} />
          ))}
      </div>
    </div>
  );
};

export default TransactionsList;
