import { useNavigate } from "react-router-dom";
import ButtonComponent from "../components/ButtonComponent";

const ErrorPage = () => {
  const navigate = useNavigate();

  return (
    <div className="flex justify-center items-center min-h-screen text-center">
      <div className="flex flex-col space-y-4 items-center">
        <h2 className="pageheading">An error has occurred</h2>
        <p>Please try again later.</p>
        <ButtonComponent onClick={() => navigate("/")}>
          Back to home
        </ButtonComponent>
      </div>
    </div>
  );
};

export default ErrorPage;
