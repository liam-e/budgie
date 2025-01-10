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
  FaSchool,
  FaGasPump,
  FaGift,
  FaDog,
  FaCut,
  FaBeer,
  FaMoneyBillWave,
  FaUmbrella,
  FaCoins,
  FaPhone,
  FaSmoking,
  FaNewspaper,
  FaHeart,
  FaRecycle,
  FaArrowRight,
  FaBicycle,
  FaBabyCarriage,
  FaTv,
  FaLaptop,
} from "react-icons/fa";

export const categoryToIconAndColors = (id) => {
  switch (id) {
    case "direct-credit":
      return {
        icon: FaArrowRight,
        backgroundColor: "#4CAF50", // Green
        iconColor: "#FFFFFF", // White
      };
    case "personal":
      return {
        icon: FaHeart,
        backgroundColor: "#E91E63", // Pink
        iconColor: "#FFFFFF", // White
      };
    case "groceries":
      return {
        icon: FaShoppingBasket,
        backgroundColor: "#FFEB3B", // Yellow
        iconColor: "#4CAF50", // Green
      };
    case "utilities":
      return {
        icon: FaScrewdriver,
        backgroundColor: "#607D8B", // Blue Grey
        iconColor: "#FFFFFF", // White
      };
    case "dining":
    case "restaurants-and-cafes":
    case "takeaway":
      return {
        icon: FaUtensils,
        backgroundColor: "#FF9800", // Orange
        iconColor: "#FFFFFF", // White
      };
    case "transport":
    case "public-transport":
    case "taxis-and-share-cars":
    case "toll-roads":
      return {
        icon: FaCar,
        backgroundColor: "#9C27B0", // Purple
        iconColor: "#FFFFFF", // White
      };
    case "rent-and-mortgage":
    case "home":
      return {
        icon: FaHome,
        backgroundColor: "#3F51B5", // Indigo
        iconColor: "#FFFFFF", // White
      };
    case "home-improvement":
    case "home-maintenance-and-improvements":
    case "homeware-and-appliances":
      return {
        icon: FaTools,
        backgroundColor: "#8BC34A", // Light Green
        iconColor: "#FFFFFF", // White
      };
    case "entertainment":
    case "taxis-and-share-cars":
    case "games-and-software":
    case "events-and-gigs":
      return {
        icon: FaFilm,
        backgroundColor: "#673AB7", // Deep Purple
        iconColor: "#FFFFFF", // White
      };
    case "shopping":
    case "clothing-and-accessories":
      return {
        icon: FaShoppingCart,
        backgroundColor: "#E91E63", // Pink
        iconColor: "#FFFFFF", // White
      };
      case "technology":
        return {
          icon: FaLaptop,
          backgroundColor: "#4287f5", // Blue
          iconColor: "#FFFFFF", // White
        };
    case "travel":
    case "holidays-and-travel":
      return {
        icon: FaPlane,
        backgroundColor: "#03A9F4", // Light Blue
        iconColor: "#FFFFFF", // White
      };
    case "fitness-and-wellbeing":
      return {
        icon: FaDumbbell,
        backgroundColor: "#4CAF50", // Green
        iconColor: "#FFFFFF", // White
      };
    case "health-and-medical":
    case "life-admin":
      return {
        icon: FaMedkit,
        backgroundColor: "#F44336", // Red
        iconColor: "#FFFFFF", // White
      };
    case "gifts-and-charity":
    case "investments":
      return {
        icon: FaGift,
        backgroundColor: "#FFC107", // Amber
        iconColor: "#FFFFFF", // White
      };
    case "pets":
      return {
        icon: FaDog,
        backgroundColor: "#795548", // Brown
        iconColor: "#FFFFFF", // White
      };
    case "hair-and-beauty":
      return {
        icon: FaCut,
        backgroundColor: "#E91E63", // Pink
        iconColor: "#FFFFFF", // White
      };
    case "car-insurance-and-maintenance":
    case "car-repayments":
      return {
        icon: FaMoneyBillWave,
        backgroundColor: "#009688", // Teal
        iconColor: "#FFFFFF", // White
      };
    case "fuel":
      return {
        icon: FaGasPump,
        backgroundColor: "#FF5722", // Deep Orange
        iconColor: "#FFFFFF", // White
      };
    case "internet":
    case "mobile-phone":
      return {
        icon: FaPhone,
        backgroundColor: "#03A9F4", // Light Blue
        iconColor: "#FFFFFF", // White
      };
    case "parking":
      return {
        icon: FaCar,
        backgroundColor: "#607D8B", // Blue Grey
        iconColor: "#FFFFFF", // White
      };
    case "hobbies":
      return {
        icon: FaHeart,
        backgroundColor: "#F06292", // Light Pink
        iconColor: "#FFFFFF", // White
      };
    case "pubs-bars-and-alcohol":
      return {
        icon: FaBeer,
        backgroundColor: "#795548", // Brown
        iconColor: "#FFFFFF", // White
      };
    case "education-and-student-loans":
      return {
        icon: FaSchool,
        backgroundColor: "#4CAF50", // Green
        iconColor: "#FFFFFF", // White
      };
    case "tobacco-and-vaping":
      return {
        icon: FaSmoking,
        backgroundColor: "#9E9E9E", // Grey
        iconColor: "#FFFFFF", // White
      };
    case "news-magazines-and-books":
      return {
        icon: FaNewspaper,
        backgroundColor: "#607D8B", // Blue Grey
        iconColor: "#FFFFFF", // White
      };
    case "lottery-and-gambling":
      return {
        icon: FaCoins,
        backgroundColor: "#FFEB3B", // Yellow
        iconColor: "#000000", // Black
      };
    case "home-insurance-and-rates":
      return {
        icon: FaUmbrella,
        backgroundColor: "#3F51B5", // Indigo
        iconColor: "#FFFFFF", // White
      };
    case "good-life":
      return {
        icon: FaRecycle,
        backgroundColor: "#8BC34A", // Light Green
        iconColor: "#FFFFFF", // White
      };
    case "transfer":
      return {
        icon: FaArrowRight,
        backgroundColor: "#607D8B", // Blue Grey
        iconColor: "#FFFFFF", // White
      };
    case "cycling":
      return {
        icon: FaBicycle,
        backgroundColor: "#607D8B", // Blue Grey
        iconColor: "#FFFFFF", // White
      };
    case "family":
      return {
        icon: FaBabyCarriage,
        backgroundColor: "#607D8B", // Blue Grey
        iconColor: "#FFFFFF", // White
      };
    case "tv-and-music":
      return {
        icon: FaTv,
        backgroundColor: "#607D8B", // Blue Grey
        iconColor: "#FFFFFF", // White
      };
    default:
      return {
        icon: FaQuestion,
        backgroundColor: "#9E9E9E", // Grey
        iconColor: "#FFFFFF", // White
      };
  }
};
