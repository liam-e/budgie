import React from "react";

const UploadCSVPage = () => {
  return (
    <form action="#" method="post" onSubmit={registerSubmitForm} className="">
      <p>Upload CSV</p>
      <input id="file" type="file" onChange={handleFileChange} />
    </form>
  );
};

export default UploadCSVPage;
