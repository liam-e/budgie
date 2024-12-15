import React from "react";
import { FaFileArrowUp, FaChartPie, FaSliders } from "react-icons/fa6";

const FeatureCard = ({ feature }) => {
  return (
    <div className="flex flex-col items-center text-center p-4">
      <div className="text-pastelGreen mb-4">{feature.icon}</div>
      <h3 className="text-lg font-semibold mb-2">{feature.heading}</h3>
      <p className="">{feature.body}</p>
    </div>
  );
};

const FeaturesSection = () => {
  const features = [
    {
      heading: "Easy Data Import",
      body: "Quickly upload your financial data from any source and start budgeting hassle-free.",
      icon: <FaFileArrowUp className="text-4xl" />,
    },
    {
      heading: "Clear Insights",
      body: "See where your money is going with simple, visual breakdowns of your spending and saving.",
      icon: <FaChartPie className="text-4xl" />,
    },
    {
      heading: "Custom Budgets",
      body: "Set limits and goals that suit your lifestyle, helping you save more and stress less.",
      icon: <FaSliders className="text-4xl" />,
    },
  ];

  return (
    <div className="py-12 px-4">
      <div className="max-w-6xl mx-auto">
        <h2 className="text-2xl font-bold text-center mb-8">Why Budgie?</h2>
        <div className="grid grid-cols-1 md:grid-cols-3 gap-8">
          {features.map((f, idx) => (
            <FeatureCard key={idx} feature={f} />
          ))}
        </div>
      </div>
    </div>
  );
};

export default FeaturesSection;
