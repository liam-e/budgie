const InputErrorMessage = ({ children }) => {
  return (
    <>
      {children ? (
        <p className="visible text-red-700 text-sm p-3">{children}</p>
      ) : (
        <p className="invisible text-sm p-3">[Placeholder]</p>
      )}
    </>
  );
};

export default InputErrorMessage;
