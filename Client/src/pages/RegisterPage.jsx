import { useState } from "react";
import { useNavigate, Link } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import ButtonComponent from "../components/ButtonComponent"; // Import the reusable button

const RegisterPage = () => {
  const { register } = useAuth();
  const navigate = useNavigate();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [formMessage, setFormMessage] = useState("");

  const registerSubmitForm = async (e) => {
    e.preventDefault();

    const passwordRegex = /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/;
    if (!passwordRegex.test(password)) {
      setFormMessage(
        "* Password must have a minimum of 8 characters with at least one letter and one number."
      );
      return;
    }

    if (password !== confirmPassword) {
      setFormMessage("* The password and confirmation password do not match.");
      return;
    }

    const newUser = {
      email,
      password,
      confirmPassword,
    };

    register(newUser);

    return navigate("/home/dashboard");
  };

  const labelStyle = "text-gray-800";
  const inputStyle = "my-1 p-2 mb-4";

  return (
    <div className="flex items-center justify-center p-5">
      <div>
        <form
          action="#"
          method="post"
          onSubmit={registerSubmitForm}
          className="flex flex-col justify-start bg-pastelYellow p-8 border-4 border-pastelGreen"
        >
          <h2 className="mt-4 mb-6">Create a new account</h2>
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
          <label className={labelStyle} htmlFor="confirm-password">
            Confirm password:
          </label>
          <input
            className={inputStyle}
            type="password"
            name="confirm-password"
            id="confirm-password"
            required
            value={confirmPassword}
            onChange={(e) => setConfirmPassword(e.target.value)}
          />
          <div className="flex flex-row justify-center w-full">
            <ButtonComponent type="submit">Create account</ButtonComponent>
          </div>
          <div className="">
            <div className="h-20">
              <p className="block w-96 overflow-hidden text-left py-2 text-red-600 text-wrap">
                {formMessage}
              </p>
            </div>
            <p>
              Already have an account? <Link to="/login">Log in</Link>
            </p>
          </div>
        </form>
      </div>
    </div>
  );
};

export default RegisterPage;
