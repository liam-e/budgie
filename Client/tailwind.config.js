/** @type {import('tailwindcss').Config} */
export default {
  content: ["./index.html", "./src/**/*.{js,ts,jsx,tsx}"],
  theme: {
    fontFamily: {
      sans: ["Poppins", "Arial"],
    },
    screens: {
      sm: "480px",
      md: "768px",
      lg: "976px",
      xl: "1440px",
    },
    extend: {
      colors: {
        pastelBlue: "#B3EBF2",
        pastelYellow: "#FDFD96",
        pastelLightYellow: "#fcfcae",
        pastelGreen: "#54de54",
        pastelLightGreen: "#7ee77e",
        pastelDarkGreen: "#22ba22",
      },
      fontFamily: {
        sans: ["Poppins", "Arial"],
      },
      height: {
        category: "40px",
      },
      width: {
        category: "100px",
      },
    },
  },
  plugins: [],
};
