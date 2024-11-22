import ClipLoader from "react-spinners/ClipLoader";

const ButtonSpinner = () => {
  return (
    <div className="flex justify-center">
      <ClipLoader
        // color={color}
        // cssOverride={override}
        loading={true}
        size={24}
        aria-label="Button Spinner"
        data-testid="button-spinner"
      />
    </div>
  );
};

export default ButtonSpinner;
