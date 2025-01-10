import { useEffect, useState } from "react";
import ButtonComponent from "../components/ButtonComponent";
import ButtonSpinner from "../components/ButtonSpinner";
import { message } from "../components/MessageContainer";
import {
  deleteIntegrationKey,
  getIntegrationKey,
  sources,
} from "../utils/feeds";

import logoUp from "../assets/images/data-sources/logo-up.png";
import logoWestpac from "../assets/images/data-sources/logo-westpac.png";
import logoAnz from "../assets/images/data-sources/logo-anz.png";
import logoCommbank from "../assets/images/data-sources/logo-commbank.png";
import logoNab from "../assets/images/data-sources/logo-nab.png";
import { FaTrash } from "react-icons/fa";
import DeleteButton from "../components/DeleteButton";
import { useAuth } from "../context/AuthContext";
import { getUserData } from "../utils/auth";

const AddFeedPage = () => {
  const { refresh, userData } = useAuth();

  const sourceLogos = {
    Up: <img src={logoUp} alt="Logo" className="h-12 w-12" />,
    Westpac: <img src={logoWestpac} alt="Logo" className="h-12 w-12" />,
    ANZ: <img src={logoAnz} alt="Logo" className="h-12 w-12" />,
    CommBank: <img src={logoCommbank} alt="Logo" className="h-12 w-12" />,
    NAB: <img src={logoNab} alt="Logo" className="h-12 w-12" />,
  };

  const [isValid, setIsValid] = useState(false);
  const [isLoading, setIsLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [accounts, setAccounts] = useState([]);
  const [loadingStates, setLoadingStates] = useState({});
  const [selectedSource, setSelectedSource] = useState(sources[0]);
  const [integrationKey, setIntegrationKey] = useState("");
  const [hasExistingKey, setHasExistingKey] = useState(false);

  useEffect(() => {
    if (selectedSource.isAvailable) {
      getIntegrationKey(selectedSource.id).then((data) => {
        setHasExistingKey(!!data);
      });
    } else {
      setHasExistingKey(false);
    }
  }, [selectedSource]);

  const handleSelectSource = (id) => {
    setSelectedSource(sources.find((s) => s.id === id));
  };

  const handleVerifyIntegrationKey = async () => {
    setIsLoading(true);
    try {
      const response = await fetch(`${selectedSource.url}/accounts`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${integrationKey}`,
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

      setIsValid(true);
      setErrorMessage("");
      return data.data;
    } catch (error) {
      setErrorMessage("An error occurred while trying to verify the API key.");
    } finally {
      setIsLoading(false);
    }
  };

  const updateIntegrationKeySource = (newIntegrationKeySource) => {
    const storedUserData = localStorage.getItem("userData");

    if (storedUserData) {
      const userData = JSON.parse(storedUserData);

      userData.integrationKeySource = newIntegrationKeySource;

      localStorage.setItem("userData", JSON.stringify(userData));

      console.log(
        "integrationKeySource updated:",
        userData.integrationKeySource
      );
    } else {
      console.log("No userData found in localStorage");
    }

    console.log(getUserData()?.integrationKeySource, userData);
  };

  const handleSelectAccount = async (accountId, name) => {
    setLoadingStates((prev) => ({ ...prev, [accountId]: true }));

    const key = {
      source: selectedSource.id,
      key: integrationKey,
      accountId,
    };

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/IntegrationKeys`,
        {
          method: "POST",
          credentials: "include",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(key),
        }
      );

      if (!response.ok) {
        setErrorMessage(
          `Failed to submit IntegrationKey for ${selectedSource.id} account "${name}"`
        );
        return;
      }

      updateIntegrationKeySource(selectedSource.id);

      message(
        `Key stored. Data will be retrieved from ${selectedSource.id} account "${name}"`
      );
      setHasExistingKey(true);
      setErrorMessage("");
    } catch (error) {
      setErrorMessage(
        `An error occurred while submitting IntegrationKey for ${selectedSource.id} account "${name}"`
      );
    } finally {
      setLoadingStates((prev) => ({ ...prev, [accountId]: false }));
    }
  };

  const handleChangeIntegrationKey = (e) => {
    setIntegrationKey(e.target.value);
  };

  const handleSubmitIntegrationKey = (e) => {
    e.preventDefault();

    handleVerifyIntegrationKey().then((data) => {
      setAccounts(data);
    });
  };

  const handleDeleteIntegrationKey = (e) => {
    deleteIntegrationKey(selectedSource.id).then((data) => {
      setHasExistingKey(false);
      updateIntegrationKeySource(null);
    });
  };

  return (
    <div className="centerboxparent">
      <div className="centerboxchild colorbox flex md:flex-row flex-col min-w-lg">
        {/* Sidebar */}
        <div className="flex flex-col space-y-4 md:w-64 md:border-r-2 border-black md:pr-4 pb-4">
          <h3 className="text-xl font-semibold">Select source</h3>
          {sources.map((source) => (
            <div
              key={source.id}
              className={`flex items-center space-x-2 p-2 cursor-pointer ${
                selectedSource.id === source.id
                  ? source.isAvailable
                    ? "bg-pastelGreen text-white" // selected/available
                    : "bg-gray-200 text-gray-800" // selected/not available
                  : source.isAvailable
                  ? "bg-white text-black" // not selected/available
                  : "bg-gray-300 text-gray-500" // not selected/not available
              }`}
              onClick={() => handleSelectSource(source.id)}
            >
              {/* Logos */}
              <div className="flex flex-row items-center space-x-2">
                {sourceLogos[source.id]}
                <h4>{source.name}</h4>
              </div>
            </div>
          ))}
        </div>

        {/* Main Content */}
        <div className="flex flex-col w-96 pl-4">
          <h2 className="text-xl font-bold mb-4">Set API Key</h2>
          {selectedSource.isAvailable ? (
            hasExistingKey ? (
              <>
                <div className="flex flex-row space-x-5 items-center">
                  <span>Key stored</span>
                  <DeleteButton onClick={handleDeleteIntegrationKey}>
                    {isLoading ? (
                      <ButtonSpinner />
                    ) : (
                      <div className="flex flex-row space-x-1 items-center">
                        <FaTrash /> <span>Delete</span>
                      </div>
                    )}
                  </DeleteButton>
                </div>
              </>
            ) : (
              <>
                <form
                  onSubmit={handleSubmitIntegrationKey}
                  className="flex flex-row items-center space-x-2 mb-4"
                >
                  <label htmlFor="integrationKey" className="text-right">
                    Up Bank API Key:
                  </label>
                  <input
                    type="text"
                    id="integrationKey"
                    value={integrationKey}
                    onChange={handleChangeIntegrationKey}
                    placeholder="Enter Up Bank API Key"
                    className="border border-pastelDarkGreen border-2 p-2"
                    required
                  />
                  <ButtonComponent className="w-16" type="submit">
                    {isLoading ? <ButtonSpinner /> : "Add"}
                  </ButtonComponent>
                </form>
                {isValid && (
                  <div className="space-y-4">
                    <h3 className="text-lg mt-4">Choose account</h3>
                    {/* <p className="text-sm text-red-600">
                  Warning: all transaction data will be replaced with
                  transaction data from the account you select
                </p> */}
                    <div className="flex flex-col w-1/2 space-y-3 p-2">
                      {accounts.map((account) => (
                        <ButtonComponent
                          key={account.id}
                          onClick={() =>
                            handleSelectAccount(
                              account.id,
                              account.attributes.displayName
                            )
                          }
                        >
                          {loadingStates[account.id] ? (
                            <ButtonSpinner />
                          ) : (
                            <>
                              {account.attributes.displayName} -{" "}
                              {"$" + account.attributes.balance.value}{" "}
                              {account.attributes.balance.currency}
                            </>
                          )}
                        </ButtonComponent>
                      ))}
                    </div>
                  </div>
                )}
                {errorMessage && <p className="text-red-600">{errorMessage}</p>}
              </>
            )
          ) : (
            <p className="text-gray-500">This source is not available.</p>
          )}
        </div>
      </div>
    </div>
  );
};

export default AddFeedPage;
