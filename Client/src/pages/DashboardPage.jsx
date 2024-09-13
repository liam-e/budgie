import RecentTransactions from "../components/RecentTransactions";
import Summary from "../components/Summary";

const DashboardPage = () => {
  return (
    <div className="mx-auto max-w-screen-xl p-4">
      <h2 className="text-4xl mb-4">Dashboard</h2>

      <div className="flex flex-col md:flex-row md:space-x-8 space-y-8">
        {/* SUMMARY */}
        <div className="w-full md:w-1/2">
          <Summary />
        </div>

        {/* RECENT TRANSACTIONS */}
        <div className="w-full md:w-1/2">
          <RecentTransactions />
        </div>
      </div>
    </div>
  );
};

export default DashboardPage;
