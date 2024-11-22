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
    <div className="flex flex-row space-x-2">
      <IconComponent size="20px" color={iconData.backgroundColor} />
      {displayName && <div>{name}</div>}
    </div>
  );
};

export default Category;
