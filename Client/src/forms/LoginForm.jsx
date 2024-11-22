import { useForm } from "react-hook-form";
import ButtonComponent from "../components/ButtonComponent";
import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import ButtonSpinner from "../components/ButtonSpinner";
import InputErrorMessage from "../components/InputErrorMessage";

const LoginForm = () => {
  const [showErrorMessage, setShowErrorMessage] = useState(false);
  const { login, isLoading } = useAuth();
  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm({
    defaultValues: import.meta.env.DEV
      ? {
          email: "user@example.com",
          password: "password1",
        }
      : {},
  });

  const onSubmit = (data) => {
    if (errors.email || errors.password) {
      setShowErrorMessage(true);
      return;
    }

    handleLogin(data);
  };

  const handleLogin = (user) => {
    login(user)
      .then(() => {
        setShowErrorMessage(false);
        setTimeout(() => {
          navigate("/home/dashboard");
        }, 1000);
      })
      .catch((error) => {
        setShowErrorMessage(true);
      });
  };

  const labelStyle = "text-gray-800";
  const inputStyle =
    "border border-gray-300 focus:border-blue-500 focus:ring-blue-500 text-gray-900 placeholder-gray-400 bg-white focus:outline-none px-3 py-2";
  const inputInvalidStyle =
    "border border-red-500 focus:border-red-500 focus:ring-red-500 bg-red-100 placeholder-red-500 bg-red-50 focus:outline-none px-3 py-2";

  return (
    <>
      <form
        className="flex flex-col py-6 px-14"
        onSubmit={handleSubmit(onSubmit)}
      >
        <div className="flex flex-col space-y-2">
          <div className="flex flex-col space-y-1">
            <label className={labelStyle} htmlFor="email">
              Email:
            </label>
            <input
              type="text"
              {...register("email", {
                required: true,
                pattern: /^\S+@\S+$/i,
              })}
              aria-invalid={!!errors?.email}
              className={errors.email ? "invalid" : ""}
            />
          </div>
          <div className="flex flex-col space-y-1">
            <label className={labelStyle} htmlFor="password">
              Password:
            </label>
            <input
              type="password"
              {...register("password", {
                required: true,
              })}
              aria-invalid={!!errors?.password}
              className={errors.password ? "invalid" : ""}
            />
          </div>
        </div>

        <InputErrorMessage>
          {showErrorMessage ? "Invalid email/password." : ""}
        </InputErrorMessage>

        <div className="flex flex-row justify-center w-full my-4">
          <ButtonComponent className="w-20" type="submit">
            {isLoading ? <ButtonSpinner /> : "Log in"}
          </ButtonComponent>
        </div>
      </form>

      <p className="text-center">
        Don't have an account? <Link to="/register">Create one here</Link>
      </p>
    </>
  );
};

export default LoginForm;
