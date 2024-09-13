import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import ButtonComponent from "../components/ButtonComponent";

const LoginPage = () => {
  const { login } = useAuth();
  const navigate = useNavigate();

  const [email, setEmail] = useState(
    process.env.NODE_ENV === "development" ? "user@example.com" : ""
  );
  const [password, setPassword] = useState(
    process.env.NODE_ENV === "development" ? "password1" : ""
  );
  const [formMessage, setFormMessage] = useState("");

  const LoginSubmitForm = async (e) => {
    e.preventDefault();

    const user = {
      email,
      password,
      confirmPassword: password,
    };

    login(user)
      .then(() => {
        navigate("/home/dashboard");
      })
      .catch((error) => {
        console.log(error);
        setFormMessage("The email or password provided is incorrect.");
      });
  };

  const labelStyle = "text-gray-800";
  const inputStyle = "my-1 p-2 mb-4";

  return (
    <div className="flex items-center justify-center p-5">
      <div>
        <form
          action="#"
          method="post"
          onSubmit={LoginSubmitForm}
          className="flex flex-col justify-start bg-pastelYellow p-8 border-4 border-pastelGreen"
        >
          <h2 className="mt-4 mb-6">Log in</h2>
          <label className={labelStyle} htmlFor="email">
            Email:
          </label>
          <input
            className={inputStyle}
            type="email"
            name="email"
            id="email"
            required
            value={email}
            onChange={(e) => setEmail(e.target.value)}
          />
          <label className={labelStyle} htmlFor="password">
            Password:
          </label>
          <input
            className={inputStyle}
            type="password"
            name="password"
            id="password"
            required
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
          <div className="flex flex-row justify-center w-full">
            <ButtonComponent type="submit">Log in</ButtonComponent>
          </div>
          <div className="">
            <div className="h-20">
              <p className="block w-96 overflow-hidden text-left py-2 text-red-600 text-wrap">
                {formMessage}
              </p>
            </div>
            <p>
              Don't have an account? <a href="/register">Create one here</a>
            </p>
          </div>
        </form>
      </div>
    </div>
  );
};

export default LoginPage;
