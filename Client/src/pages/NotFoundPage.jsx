import { useNavigate } from "react-router-dom";
import ButtonComponent from "../components/ButtonComponent";

const NotFoundPage = () => {
  const navigate = useNavigate();
  return (
    <div className="flex flex-col items-center justify-center min-h-full space-y-8 p-12">
      <div className="flex flex-col space-y-4 items-center">
        <h2 className="pageheading">Oops! Page not found</h2>
        <p>It looks like the page you're trying to visit doesn't exist.</p>
        <ButtonComponent onClick={() => navigate("/")}>
          Take me back home
        </ButtonComponent>
      </div>
    </div>
  );
};

export default NotFoundPage;
