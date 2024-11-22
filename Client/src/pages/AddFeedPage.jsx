import React, { useState } from "react";
import ButtonComponent from "../components/ButtonComponent";
import ButtonSpinner from "../components/ButtonSpinner";

const AddFeedPage = () => {
  const [apiKey, setApiKey] = useState("");
  const [isValid, setIsValid] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [accounts, setAccounts] = useState([]);

  const handleApiKeyChange = (e) => {
    setApiKey(e.target.value);
  };

  const verifyApiKey = async () => {
    setIsLoading(true);
    const upBankUrl = "https://api.up.com.au/api/v1/accounts";
    try {
      const response = await fetch(upBankUrl, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${apiKey}`,
        },
      });

      if (!response.ok) {
        setIsValid(false);
        setErrorMessage(
          "Invalid API key or failed to connect to the Up Bank API."
        );
        return;
      }

      const data = await response.json();
      setAccounts(data.data);
      setIsValid(true);
      setIsLoading(false);
      setErrorMessage("");
    } catch (error) {
      setIsValid(false);
      setErrorMessage("An error occurred while trying to verify the API key.");
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    verifyApiKey();
  };

  const handleSelectAccount = () => {};

  return (
    <div className="flex flex-col p-8 mt-12 space-y-6 align-center items-center bg-pastelYellow border-4 border-pastelGreen max-w-lg mx-auto">
      <h2 className="text-2xl mb-6">Add Data Feed</h2>

      <form
        onSubmit={handleSubmit}
        className="flex flex-row items-center space-x-2"
      >
        <label htmlFor="apiKey" className="text-right">
          Up Bank API Key:
        </label>
        <input
          type="text"
          id="apiKey"
          value={apiKey}
          onChange={handleApiKeyChange}
          placeholder="Enter Up Bank API Key"
          className="border border-pastelDarkGreen border-2 p-2"
          required
        />

        <ButtonComponent className="w-16" type="submit">
          {isLoading ? <ButtonSpinner /> : "Add"}
        </ButtonComponent>
      </form>

      <div className="mt-6 text-center">
        {isValid && (
          <div className="space-y-4">
            <h3 className="text-lg mt-4">Choose account</h3>
            <p className="text-sm text-red-600">
              Warning: all transaction data will be replaced with transaction
              data from the account you select
            </p>
            <div className="flex flex-col m-auto w-3/6 space-y-3 p-2">
              {accounts.map((account) => (
                <ButtonComponent key={account.id} onClick={handleSelectAccount}>
                  {account.attributes.displayName} -{" "}
                  {"$" + account.attributes.balance.value}{" "}
                  {account.attributes.balance.currency}
                </ButtonComponent>
              ))}
            </div>
          </div>
        )}

        {errorMessage && <p className="text-red-600">{errorMessage}</p>}
      </div>
    </div>
  );
};

export default AddFeedPage;
