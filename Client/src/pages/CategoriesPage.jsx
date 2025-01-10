import { useLoaderData } from "react-router-dom";
import Category from "../components/Category";

const CategoriesPage = () => {
  const { categories, groupedCategories } = useLoaderData();

  return (
    <div className="space-y-6">
      <h2 className="pageheading">Categories</h2>
      {Object.entries(groupedCategories).map(([parentId, group]) => (
        <div key={parentId} className="pb-4 mb-4">
          <h3 className="text-xl mb-2">
            {categories.filter((c) => c.id === parentId)[0]?.name ||
              "Main Categories"}
          </h3>
          <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-3">
            {group.map((category) => (
              <div
                className="flex flex-row items-center p-4 bg-pastelYellow"
                key={category.id}
              >
                <div className="p-2">
                  <Category id={category.id} />
                </div>
                <div className="ml-2">
                  <div className="">{category.name}</div>
                  <div className="text-sm text-gray-600">
                    {category.transactionTypeName}
                  </div>
                </div>
              </div>
            ))}
          </div>
        </div>
      ))}
    </div>
  );
};

const categoriesLoader = async ({ params }) => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/Transactions/Categories`,
      {
        method: "GET",
        credentials: "include",
      }
    );

    if (!response.ok) {
      console.error(response.statusText || "Failed to fetch categories");
      return {};
    }

    const categories = await response.json();

    categories.sort((a, b) => a.name.localeCompare(b.parentId));

    const groupedCategories = categories.reduce((acc, category) => {
      let { parentId } = category;

      if (!acc[parentId]) {
        acc[parentId] = [];
      }
      acc[parentId].push(category);
      return acc;
    }, {});

    const mapCategoryIdToName = categories.reduce((acc, c) => {
      acc[c.id] = c.name;
      return acc;
    }, {});

    return {
      categories,
      groupedCategories,
      mapCategoryIdToName,
    };
  } catch (error) {
    console.error("Error fetching categories:", error);
    return {};
  }
};

export { CategoriesPage as default, categoriesLoader };
