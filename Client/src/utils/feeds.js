export const sources = [
  {
    id: "Up",
    name: "Up Bank",
    url: "https://api.up.com.au/api/v1",
    isAvailable: true,
  },
  {
    id: "Westpac",
    name: "Westpac",
    url: "",
    isAvailable: false,
  },
  {
    id: "ANZ",
    name: "ANZ",
    url: "",
    isAvailable: false,
  },
  {
    id: "CommBank",
    name: "CommBank",
    url: "",
    isAvailable: false,
  },
  {
    id: "NAB",
    name: "NAB",
    url: "",
    isAvailable: false,
  },
];

export const mapUpCategoryToCategory = (category) => {
  switch (category) {
    case "booze":
    case "pubs-and-bars":
      return "pubs-bars-and-alcohol";
    case "adult":
      return "none";
    case "Transfer":
    case "Scheduled Transfer":
      return "transfer";
    case "Direct Credit":
    case "Payment":
      return "direct-credit";
    case "Purchase":
      return "none";
    default:
      return category;
  }
};

export const mapUpTransactionToTransaction = (t, id, userId) => {
  const createdAt = t.attributes.createdAt
    ? new Date(t.attributes.createdAt).toISOString()
    : null;

  return {
    id: id,
    userId: userId,
    date: createdAt,
    description: t.attributes.rawText ?? t.attributes.description,
    amount: parseFloat(t.attributes.amount.valueInBaseUnits / 100),
    currency: t.attributes.amount.currencyCode,
    categoryId: mapUpCategoryToCategory(
      t.relationships.category?.data?.id || t.attributes.transactionType
    ),
    createdAt: createdAt,
    updatedAt: createdAt,
  };
};

export const getIntegrationKey = async (sourceId) => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/IntegrationKeys?source=${sourceId}`,
      {
        method: "GET",
        credentials: "include",
      }
    );

    if (response.status === 404) {
      // Return null silently if the resource is not found
      return null;
    }

    if (!response.ok) {
      throw new Error(response.statusText || "Failed to fetch IntegrationKey");
    }

    const data = await response.json();
    return data;
  } catch (error) {
    if (error.message !== "Failed to fetch IntegrationKey") {
      console.error("Error fetching IntegrationKey:", error);
    }
    return null;
  }
};

export const deleteIntegrationKey = async (sourceId) => {
  try {
    const response = await fetch(
      `${import.meta.env.VITE_API_URL}/IntegrationKeys?source=${sourceId}`,
      {
        method: "DELETE",
        credentials: "include",
      }
    );

    if (!response.ok) {
      throw new Error(response.statusText || "Failed to delete IntegrationKey");
    }

    const data = await response.json();
    return data;
  } catch (error) {
    console.error("Error deleting IntegrationKey:", error);
    return null;
  }
};
