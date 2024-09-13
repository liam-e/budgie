import React, { useEffect, useState } from "react";
import {
  FaShoppingBasket,
  FaScrewdriver,
  FaUtensils,
  FaCar,
  FaHome,
  FaTools,
  FaFilm,
  FaShoppingCart,
  FaPlane,
  FaDumbbell,
  FaMedkit,
  FaQuestion,
  FaHourglassHalf,
  FaGamepad,
  FaTshirt,
  FaBicycle,
  FaSchool,
  FaGasPump,
  FaGift,
  FaDog,
  FaBus,
  FaCut,
  FaPiggyBank,
  FaBeer,
  FaMoneyBillWave,
  FaUmbrella,
  FaThermometerHalf,
  FaCoins,
  FaRoad,
  FaHandHoldingUsd,
  FaHamburger,
  FaPhone,
  FaSmoking,
  FaNewspaper,
  FaTv,
  FaHeart,
  FaRecycle,
  FaArrowRight,
} from "react-icons/fa";

const Category = ({ categoryName: name }) => {
  const [icon, setIcon] = useState(<FaHourglassHalf />);

  useEffect(() => {
    const categoryToIcon = (name) => {
      switch (name) {
        case "Groceries":
          return <FaShoppingBasket />;
        case "Utilities":
          return <FaScrewdriver />;
        case "Dining":
        case "Restaurants & Cafes":
        case "Takeaway":
          return <FaUtensils />;
        case "Transport":
        case "Public Transport":
        case "Taxis & Share Cars":
        case "Toll Roads":
          return <FaCar />;
        case "Rent & Mortgage":
        case "Home":
          return <FaHome />;
        case "Home Improvement":
        case "Maintenance & Improvements":
        case "Homeware & Appliances":
          return <FaTools />;
        case "Entertainment":
        case "TV, Music & Streaming":
        case "Apps, Games & Software":
        case "Events & Gigs":
          return <FaFilm />;
        case "Shopping":
        case "Clothing & Accessories":
        case "Technology":
          return <FaShoppingCart />;
        case "Travel":
        case "Holidays & Travel":
          return <FaPlane />;
        case "Fitness":
        case "Fitness & Wellbeing":
          return <FaDumbbell />;
        case "Healthcare":
        case "Health & Medical":
        case "Life Admin":
          return <FaMedkit />;
        case "Gifts & Charity":
        case "Investments":
          return <FaGift />;
        case "Pets":
          return <FaDog />;
        case "Hair & Beauty":
          return <FaCut />;
        case "Car Insurance, Rego & Maintenance":
        case "Repayments":
          return <FaMoneyBillWave />;
        case "Fuel":
          return <FaGasPump />;
        case "Internet":
        case "Mobile Phone":
          return <FaPhone />;
        case "Parking":
          return <FaCar />;
        case "Hobbies":
          return <FaHeart />;
        case "Pubs & Bars":
        case "Booze":
          return <FaBeer />;
        case "Education & Student Loans":
          return <FaSchool />;
        case "Tobacco & Vaping":
          return <FaSmoking />;
        case "News, Magazines & Books":
          return <FaNewspaper />;
        case "Adult":
          return <FaHeart />;
        case "Lottery & Gambling":
          return <FaCoins />;
        case "Home Insurance & Rates":
        case "Rates & Insurance":
          return <FaUmbrella />;
        case "Good Life":
          return <FaRecycle />;
        case "Transfer":
          return <FaArrowRight />;
        default:
          return <FaQuestion />;
      }
    };

    setIcon(categoryToIcon(name));
  }, [name]);

  return icon;
};

export default Category;
