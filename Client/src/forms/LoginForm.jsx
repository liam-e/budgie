import { useForm } from "react-hook-form";
import ButtonComponent from "../components/ButtonComponent";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import ButtonSpinner from "../components/ButtonSpinner";
import InputErrorMessage from "../components/InputErrorMessage";
import LinkComponent from "../components/LinkComponent";

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

  return (
    <>
      <form
        className="flex flex-col py-6 px-14"
        onSubmit={handleSubmit(onSubmit)}
      >
        <div className="flex flex-col space-y-2">
          <div className="flex flex-col space-y-1">
            <label className="" htmlFor="email">
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
            <label className="" htmlFor="password">
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
        Don't have an account?{" "}
        <LinkComponent to="/register">Create one here</LinkComponent>
      </p>
    </>
  );
};

export default LoginForm;
