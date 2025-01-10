import { useState } from "react";
import { FaUpload } from "react-icons/fa";
import ButtonComponent from "../components/ButtonComponent";
import { useNavigate } from "react-router-dom";
import { message } from "../components/MessageContainer";
import InputErrorMessage from "../components/InputErrorMessage";
import InputMessage from "../components/InputMessage";

const UploadCSVPage = () => {
  const [selectedFile, setSelectedFile] = useState(null);
  const [fileError, setFileError] = useState("");
  const navigate = useNavigate();

  const handleFileChange = (event) => {
    const file = event.target.files[0];

    if (file && file.type === "text/csv") {
      setSelectedFile(file);
      setFileError("");
    } else {
      setFileError("Please select a valid CSV file.");
      setSelectedFile(null);
    }
  };

  const handleUpload = async () => {
    if (!selectedFile) {
      setFileError("No file selected.");
      return;
    }

    const formData = new FormData();
    formData.append("file", selectedFile);

    try {
      const response = await fetch(
        `${import.meta.env.VITE_API_URL}/Transactions/UploadCsv`,
        {
          method: "POST",
          credentials: "include",
          body: formData,
        }
      );

      if (!response.ok) {
        throw new Error(`Upload failed with status ${response.status}`);
      }

      const data = await response.text();

      message(data);

      navigate("/home/dashboard?uploadedCSV=true");
    } catch (error) {
      console.error("File upload error:", error);
      setFileError("File upload failed. Please try again.");
    }
  };

  return (
    <div className="centerboxparent">
      <div className="centerboxchild colorbox">
        <div className="flex flex-col p-12 space-y-6 align-center items-center">
          <FaUpload size="40px" />
          <p className="text-lg text-center">Select the CSV file to upload:</p>

          <div className="flex flex-col items-center">
            <input
              type="file"
              accept=".csv"
              onChange={handleFileChange}
              className="mt-4 w-60 mx-auto inline-block"
            />
            {selectedFile ? (
              <InputMessage>File selected: {selectedFile.name}</InputMessage>
            ) : (
              <InputErrorMessage>{fileError}</InputErrorMessage>
            )}
          </div>
          <ButtonComponent onClick={handleUpload}>Upload File</ButtonComponent>
        </div>
      </div>
    </div>
  );
};

export default UploadCSVPage;
