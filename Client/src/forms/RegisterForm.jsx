import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";
import { useForm } from "react-hook-form";
import ButtonComponent from "../components/ButtonComponent";
import InputErrorMessage from "../components/InputErrorMessage";
import InputMessage from "../components/InputMessage";
import ButtonSpinner from "../components/ButtonSpinner";
import { useState } from "react";
import LinkComponent from "../components/LinkComponent";

const RegisterForm = () => {
  const { signUp, isLoading } = useAuth();
  const [emailConflictErrorMessage, setEmailConflictErrorMessage] =
    useState("");

  const navigate = useNavigate();

  const {
    register,
    handleSubmit,
    getValues,
    formState: { errors },
  } = useForm({
    defaultValues: import.meta.env.DEV
      ? {
          firstName: "John",
          email: `user${Math.floor(Math.random() * 10000)}@example.com`,
          password: "password1",
          confirmPassword: "password1",
        }
      : {},
  });

  const onSubmit = (data) => {
    handleSignUp(data);
  };

  const handleSignUp = (newUser) => {
    setEmailConflictErrorMessage("");

    signUp(newUser)
      .then((data) => {
        navigate("/home/add-data?newuser=true");
      })
      .catch((error) => {
        if (error.status === 409) {
          console.log("Conflict error:", error);
          setEmailConflictErrorMessage("Email already exists");
        } else {
          console.error("Unhandled error:", error);
        }
      });
  };

  return (
    <>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="flex flex-col md:flex-row md:space-x-6 content-between p-4 h-full ">
          <div className="flex flex-col space-y-4 w-full md:w-1/2">
            {/* FIRST NAME */}
            <div className="flex flex-col space-y-1">
              <label htmlFor="firstName">First name:</label>
              <input
                type="text"
                {...register("firstName", {
                  required: {
                    value: true,
                    message: "Please enter your first name",
                  },
                })}
                aria-invalid={errors.firstName ? "true" : "false"}
                className={errors.firstName ? "error" : ""}
              />
              <InputErrorMessage>
                {errors.firstName ? errors.firstName.message : ""}
              </InputErrorMessage>
            </div>

            {/* LAST NAME */}
            <div className="flex flex-col space-y-1">
              <label htmlFor="lastName">Last name:</label>
              <input
                type="text"
                {...register("lastName")}
                aria-invalid={errors.lastName ? "true" : "false"}
                className={errors.lastName ? "error" : ""}
              />
              <InputErrorMessage>
                {errors.LastName ? errors.LastName.message : ""}
              </InputErrorMessage>
            </div>

            {/* EMAIL */}
            <div className="flex flex-col space-y-1">
              <label htmlFor="email">Email:</label>
              <input
                type="text"
                {...register("email", {
                  required: {
                    value: true,
                    message: "Please enter your email address",
                  },
                  pattern: {
                    value: /^\S+@\S+$/i,
                    message: "Invalid email format",
                  },
                })}
                aria-invalid={errors.email ? "true" : "false"}
                className={errors.email ? "error" : ""}
              />
              <InputErrorMessage>
                {emailConflictErrorMessage ||
                  (!errors.firstName && errors.email
                    ? errors.email.message
                    : "")}
              </InputErrorMessage>
            </div>
          </div>

          <div className="flex flex-col space-y-4 w-full md:w-1/2">
            {/* PASSWORD */}
            <div className="flex flex-col space-y-1">
              <label htmlFor="password">Password:</label>
              <input
                type="password"
                {...register("password", {
                  required: { value: true, message: "Password is required" },
                  pattern: {
                    value: /^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{8,}$/,
                    message: "Invalid password format",
                  },
                })}
                aria-invalid={errors.password ? "true" : "false"}
                className={errors.password ? "error" : ""}
              />

              <InputMessage>
                {
                  "Minimum of 8 characters with at least one letter and one number."
                }
              </InputMessage>
              <InputErrorMessage>
                {!errors.firstName && !errors.email && errors.password
                  ? errors.password.message
                  : ""}
              </InputErrorMessage>
            </div>

            {/* CONFIRM PASSWORD */}
            <div className="flex flex-col space-y-1">
              <label htmlFor="confirm-password">Confirm password:</label>
              <input
                type="password"
                {...register("confirmPassword", {
                  required: {
                    value: true,
                    message: "Please confirm the password.",
                  },
                  validate: {
                    equalsPassword: (value) => value === getValues().password,
                  },
                })}
                aria-invalid={errors.confirmPassword ? "true" : "false"}
                className={errors.confirmPassword ? "error" : ""}
              />
              <InputErrorMessage>
                {!errors.firstName &&
                !errors.email &&
                !errors.password &&
                errors.confirmPassword
                  ? errors.confirmPassword.type === "equalsPassword"
                    ? "Does not match"
                    : errors.confirmPassword.message
                  : ""}
              </InputErrorMessage>
            </div>
          </div>
        </div>

        <div className="flex flex-row justify-center w-full my-4">
          <ButtonComponent className="w-40" type="submit">
            {isLoading ? <ButtonSpinner /> : "Create account"}
          </ButtonComponent>
        </div>
      </form>
      <p className="text-center">
        Already have an account?{" "}
        <LinkComponent to="/login">Log in</LinkComponent>
      </p>
    </>
  );
};

export default RegisterForm;
