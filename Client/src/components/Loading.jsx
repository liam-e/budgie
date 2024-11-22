import ClipLoader from "react-spinners/ClipLoader";

const Loading = () => {
  const colorPastelGreen = "#54de54"; // TODO: reference from tailwind
  return (
    <div className="flex items-center justify-center w-screen h-screen">
      <ClipLoader
        color={colorPastelGreen}
        loading={true}
        size={50}
        aria-label="Loading Spinner"
        data-testid="loader"
      />
    </div>
  );
};

export default Loading;
