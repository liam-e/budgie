import React, { useEffect, useState } from "react";
import { FaHourglassHalf } from "react-icons/fa";
import { categoryToIconAndColors } from "../utils/categoryIcon";

const Category = ({ id, name, displayName = false }) => {
  const [iconData, setIconData] = useState({
    icon: FaHourglassHalf,
    backgroundColor: "#E0E0E0",
    iconColor: "#000000",
  });

  useEffect(() => {
    const { icon, backgroundColor, iconColor } = categoryToIconAndColors(id);
    setIconData({ icon, backgroundColor, iconColor });
  }, [id]);

  const IconComponent = iconData.icon;

  return (
    <div className="flex items-center space-x-2">
      {/* Icon */}
      <div
        className="w-8 h-8 flex items-center justify-center"
        style={{
          backgroundColor: iconData.backgroundColor,
          borderRadius: "50%", // Circular icon background
        }}
      >
        <IconComponent size="16px" color={iconData.iconColor} />
      </div>

      {/* Name */}
      {displayName && (
        <div
          className="flex items-center text-left line-clamp-2 w-36"
          // style={{
          //   width: "100px", // Fixed width for consistency
          //   height: "40px", // Fixed height for 1-2 lines of text
          // }}
        >
          <span
            className="line-clamp-2 overflow-hidden text-sm"
            style={{
              display: "-webkit-box",
              WebkitLineClamp: 2,
              WebkitBoxOrient: "vertical",
            }}
          >
            {name}
          </span>
        </div>
      )}
    </div>
  );
};

export default Category;
