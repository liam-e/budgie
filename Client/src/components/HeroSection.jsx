import React from "react";
import { Link } from "react-router-dom";

const HeroSection = () => {
  return (
    <div className="flex flex-col md:flex-row items-center justify-between p-5">
      {/* Text Section */}
      <div className="md:w-1/2 flex flex-col justify-center p-8 space-y-12">
        <div className="w-1/4">
          <h1 className="text-6xl font-bold uppercase leading-normal italic">
            Free your money
          </h1>
        </div>
        <p className="text-gray-800 text-xl leading-relaxed">
          Take control of your cash, smash those savings goals, and track your
          spending with ease.
        </p>
        <div class="flex justify-end w-full">
          <Link
            to="/register"
            className="bg-pastelGreen text-black border-2 border-black px-4 py-3 no-underline"
          >
            Get Started
          </Link>
        </div>
      </div>

      {/* Graphic Section */}
      <div className="hidden md:block md:w-1/2 p-8 w-full">
        {/* <img
          src="your-graphic-url.png"
          alt="Budgeting graphic"
          className="w-full h-auto"
        /> */}
      </div>
    </div>
  );
};

export default HeroSection;
